using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omu.ValueInjecter;

namespace SerializersCompare.Serializers.Thrift
{
    
    // Thrift doesn't have float (32bit) just double (64bits). Our injection framework ("Value Injecter")
    // automagically injects values if the name and type match - that breaks here because although name matches,
    // the types don't. So we have extension helpers to make it match    

    class FloatToDouble : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return (c.SourceProp.Type == typeof (float) && c.TargetProp.Type == typeof (double));
        }
    }

    class DoubleToFloat : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            return (c.SourceProp.Type == typeof(double) && c.TargetProp.Type == typeof(float));
        }
        protected override object SetValue(ConventionInfo c)
        {
            var dblStr = c.SourceProp.Value.ToString();
            var floatVal = float.Parse(dblStr);
            return floatVal;
        }
    }

}
