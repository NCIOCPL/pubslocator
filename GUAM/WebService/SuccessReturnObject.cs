using System;

namespace WebService
{
    [Serializable]
    public class SuccessReturnObject : ReturnObject
    {
        public SuccessReturnObject()
        {
            this.ReturnCode = 0;
        }

        public SuccessReturnObject(object p_ReturnValue)
        {
            this.ReturnValue = p_ReturnValue;
        }
    }
}