using System;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// 方法描述自定义特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class FunctionDescriptionAttribute : Attribute
    {
        /// <summary>
        /// this is only singleton object you can get
        /// </summary>
        public static readonly FunctionDescriptionAttribute Default = new FunctionDescriptionAttribute();

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
        public virtual string Description => DescriptionValue;

        public string DescriptionValue { get; set; }

        public override bool Equals(object? obj) =>
            obj is FunctionDescriptionAttribute other && other.Description == Description;

        public override int GetHashCode() => Description?.GetHashCode() ?? 0;

        public override bool IsDefaultAttribute() => Equals(Default);
    }
}