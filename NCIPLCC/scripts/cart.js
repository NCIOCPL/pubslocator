//Displays a confirmation message to the user, before removing an item from shopping cart.
function fnAskBeforeRemoval(ctrlId) 
{
    var obj = document.getElementById(ctrlId);
    if (obj != null) 
    {
        if (confirm("Do you wish to remove this item?"))
            return true;
        else
            return false;
    }
    return true;
}
