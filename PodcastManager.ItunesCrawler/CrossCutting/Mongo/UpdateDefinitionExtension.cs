using System.Linq.Expressions;
using MongoDB.Driver;

namespace PodcastManager.ItunesCrawler.CrossCutting.Mongo;

public static class UpdateDefinitionExtension
{
    public static UpdateDefinition<T> SetOrUnset<T, TField>(this UpdateDefinitionBuilder<T> @this,
        Expression<Func<T, object>> field, TField value) =>
        value == null 
            ? @this.Unset(field)
            : @this.Set(field, value);
    public static UpdateDefinition<T> SetOrUnset<T, TField>(this UpdateDefinition<T> @this,
        Expression<Func<T, object>> field, TField value) =>
        value == null 
            ? @this.Unset(field)
            : @this.Set(field, value);
}