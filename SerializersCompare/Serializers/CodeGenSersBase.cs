using Omu.ValueInjecter;

namespace SerializersCompare.Serializers
{
    public abstract class CodeGenSersBase<T, U> : SerializerBase<T>
        where T : new()
        where U : new()
    {
        protected U CodeGenObjSer;
        protected T RegenAppObj;

        /// <summary>
        /// Unlike other serializers, Thrift code-gens it's own data classes and those, not the 
        /// application's existing classes are used in transport and RPC. So most real world 
        /// projects will certainly bear some extra processing time taken to copy values from
        /// their internal app logic classes into the Thrift auto-gen'd classes.
        /// In otehr serializers one can directly use the app logic classes, bypassing the 
        /// extra copy needed by Thrift.
        /// 
        /// So "Cheating" simply means whether or not we should exclude the data-copy times from Thrift's timings. 
        /// Real world performance should be with cheating disabled.
        /// 
        /// Note: The efficiency of the chosen injection/projection library is worth 
        /// questioning. We're using ValueInjecter for now, perhaps AutoMapper is quicker?
        /// </summary>
        protected bool EnableCheating;

        public CodeGenSersBase(bool enableCheating = false)
        {
            EnableCheating = enableCheating;
        }

        public new virtual string Name()
        {
            if (EnableCheating)
                SerName += " (cheating)";
            return SerName;
        }


        protected virtual T FromSerObject(U regenTMsg)
        {
            RegenAppObj = new T();
            RegenAppObj.InjectFrom(regenTMsg); // inject most values                
            return RegenAppObj;
        }

        protected virtual U ToSerObject(T thisObj)
        {
            CodeGenObjSer = new U();
            CodeGenObjSer.InjectFrom(thisObj); // inject most values
            return CodeGenObjSer;
        }
    }
}
