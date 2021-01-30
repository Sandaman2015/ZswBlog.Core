using System;

namespace ZswBlog.Common
{
    /// <summary>
    /// 方法描述自定义特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class FunctionDescriptionAttribute : Attribute
    {
        /// <summary>
        /// this is only singleton object you can get
        /// </summary>
        private static readonly FunctionDescriptionAttribute Default = new FunctionDescriptionAttribute();

        /// <summary>
        /// on params constructor 
        /// </summary>
        public FunctionDescriptionAttribute() : this(string.Empty)
        {
        }

        /// <summary>
        /// params constructor
        /// </summary>
        /// <param name="description">function description</param>
        public FunctionDescriptionAttribute(string description)
        {
            DescriptionValue = description;
        }

        /// <summary>
        /// get your function description when you are logging 
        /// </summary>
        public string Description => DescriptionValue;
        /// <summary>
        /// 
        /// </summary>
        public string DescriptionValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) =>
            obj is FunctionDescriptionAttribute other && other.Description == Description;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Description?.GetHashCode() ?? 0;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool IsDefaultAttribute() => Equals(Default);
    }
}