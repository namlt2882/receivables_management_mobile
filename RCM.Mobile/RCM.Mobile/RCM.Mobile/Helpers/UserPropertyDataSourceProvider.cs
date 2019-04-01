using RCM.Mobile.Models;
using System.Collections;
using System.Linq;
using Telerik.XamarinForms.Input.DataForm;


namespace RCM.Mobile.Helpers
{
    public class UserPropertyDataSourceProvider : PropertyDataSourceProvider
    {
        public override IList GetSourceForKey(object key)
        {
            if (key.ToString().Equals(nameof(Receivable.Id)))
            {
                return Enumerable.Range(1, 10).ToList();
            }

            return base.GetSourceForKey(key);
        }
    }

}
