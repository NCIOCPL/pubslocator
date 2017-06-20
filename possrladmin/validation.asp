<%
	'on error resume next
	'***TODO: Configure virtual dir(IIS) to use a general error page.  Focus on the 500 error pages.
	const VTYPEGENERAL	= 0 'General (check for any dangerous character)
	const VTYPENUMERIC	= 1 'Numeric (zip5,counting values and includes the hyphen for negative values
	const VTYPNAME		= 2 'Name
	const VTYPEEMAIL	= 3 'Email 
	const VYPESTATE		= 4 'State
	const VTYPEPHONE	= 5 'Phone 
	const VTYPEADDRESS	= 6 'Address
	const VTYPEDATE		= 7 'Date no space


    dim regEx
    dim ErrorPage
    dim PageValidation
    dim BadTokens

	'***TODO: Change the values below based on business need
	ErrorPage = "ErrorPage.asp"		'point to your generic errorpage handler    
	ValidateRequest = true			'similar to 'ValidateRequest' in .Net
	'ValidateTokens  = true			'either you change the value here or manipulate the 'BadTokens' array
	
	
	
	Set regEx = New RegExp    
	regEx.Global = true	

	
	'*** mandatory check on the 3 request objects	
	if ValidateRequest then	
		session("__errormessage") = "HTML tags detected"
		regEx.Pattern = "[<>]"	'avoiding html tags
		'forms
		For Each item in Request.Form 
			If ( regEx.Test(Request(item)) ) Then Response.Redirect(ErrorPage)	
		Next
		'querystrings
		For Each item in Request.QueryString 
			If ( regEx.Test(Request(item)) ) Then Response.Redirect(ErrorPage)	
		Next
		'cookies
		For Each item in Request.Cookies 
			If ( regEx.Test(Request(item)) ) Then Response.Redirect(ErrorPage)	
		Next
	end if


	'*** list of words you do not want to be submitted to the business layer
	'*** lowercase values preferred, no spaces
	BadTokens = Array("--", ";", "/*", "*/",_
		"declare", "delete", "drop",  "exec", "execute", "fetch", "insert", "kill", "open",_
		"xp_","sp_","sysobjects", "syscolumns", "@@", "@", _
		"<applet>", "<body>", "<embed>", "<frame>", "<script>", "<frameset>", "<html>", "<iframe>", "<img>", "<style>", "<layer>", "<link>", "<ilayer>", "<meta>", "<object>" )

		'*** other bad tokens you might consider
		'"begin", "end", "cast", "create", "cursor","dual","char", "nchar", "varchar", "nvarchar",_
		'"select",  "alter", "table", "update", _



	'*** true if the str contains any of the tokens above, false otherwise
	Function ContainsBadTokens(str) 
		On Error Resume Next   
		Dim lstr 
		dim s	'added recently 10/3 cause fo possible conflict
		dim a	'added recently 10/3 cause fo possible conflict (untested)
		ContainsBadTokens = false

		For Each s in BadTokens  
			for Each a in split(LCase(str)," " )	'***You can put more delimiters if you want
				if a = s then
					ContainsBadTokens = true
					Exit Function		
				end if
			next
		next 
	End Function 




'==================================================================================================
' intType:  1: ZIP (numbers only)
'           0: 0 = General (check for any dangerous character)
'           2: Name 
'           3: Email 
'           4: Address
'           5: State
'			6: Phone 
'
' intMaxLen - maximum length.
'           - If intMaxLen = 0 then don't check for length
'
' Returns:  0 - success
'           1 - length
'           2 - wrong type (numeric vs non-numeric)
'           3 - bad tokens present
'================================================================================================== 



Public Function Validate(strToCheck, intType, intMaxLen, bBadTokenCheck) 
    Dim regEx
    Dim intReturn
	
	
	intReturn = 0
	
	'*** Step 1: Check length if needed
	if (intMaxLen > 0) and (len(strToCheck) > intMaxLen) then
		intReturn = 1	'problem w/ length
	end if
	

	'*** Step 2: Check chars on the control level (differentiate between date fields vs. address field)
    if intReturn = 0 then
       Set regEx = New RegExp
	   regEx.Global = true	
	   
	   
	   Select Case intType
	
		'***TODO: use http://www.myregextester.com/index.php to doublecheck
	       case 1 ' VTYPENUMERIC pos, negative, ints, floats
	           regEx.Pattern = "[^0-9.]"

	       case 2 'VTYPENAME [name] only letters and "_.,'&/()" characters
	           regEx.Pattern = "[^0-9a-zA-Z-_.,'&/()]" 

	       case 3 'VTYPEEMAIL  only letters, numbers and "_@." characters
	           regEx.Pattern = "[^0-9a-zA-Z-_@.]"  
	           'regEx.Pattern = "\w+[@]+\w"

	       case 4 ' VYPESTATE only letters
	           regEx.Pattern = "[^a-zA-Z]" 

	       case 5 ' VTYPEPHONE [phone]
	           regEx.Pattern = "[^0-9a-zA-Z-(),]" 

	       case 6 'VTYPEADDRESS [address] [city] only letters, number and "@._#,()/" characters
	           regEx.Pattern = "[^0-9a-zA-Z-@.'_#,()/]"  
	       case 7 'number and '/'
	           regEx.Pattern = "[^0-9/]"  
    
		   case else	' [ran into unknown type ... assume its a general type]
				regEx.Pattern = "[^0-9a-zA-Z[]{},.?![]{}/=-@#$%^&*()_+]"				
	   end select  

	    if regEx.Test(replace(trim(strToCheck), " ", "")) = true then
		    intReturn = 2
        end if

	end if



	'*** Step 3: Check for bad tokens if needed
    if (intReturn = 0 and bBadTokenCheck and ContainsBadTokens(strToCheck)) then
		intReturn = 3
    end if

	'*** Set up the value of Validate or bug out to errorpage.asp if you need to.
	'*** The important thing here is to prevent the value reaching the business layer.
	select case intReturn
		case 0:
			    Validate = strToCheck	'*** Return original string
		case 1:
				session("__errormessage") = "Value exceeded max length"
				Response.Redirect(ErrorPage)
		case 2:
				session("__errormessage") = "Input is not in expected format"
				Response.Redirect(ErrorPage)	
		case else:	
				session("__errormessage") = "Bad tokens in input"
				Response.Redirect(ErrorPage)	
	end select



	'*** At this point, we know strToCheck is 'clean' so its ok to exit function
     
end function

%>
