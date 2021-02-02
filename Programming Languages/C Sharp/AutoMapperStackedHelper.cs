using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class AutoMapperStackedHelper
    {
        /// <summary>
        /// You can stack multiple inputs into a mapping for a single output
        /// Mapper.AutoMapper.Mapper.Map<OutputModel>(InputModel1).Map(InputModel2).Map(InputModel3)
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source)
        {
            if (source == null) { return destination; } //Don't do a mapping if the source is null
            return Mapper.AutoMapper.Mapper.Map(source, destination);
        }
    }
}
