﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc.Infrastructure;
using CodeComb.AspNet.Localization;
using CodeComb.AspNet.Localization.EntityFramework;
using CodeComb.AspNet.Localization.Json;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class LocalizationExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection self)
        {
            return self.AddSingleton<ILocalizationStringCollection, JsonCollection>();
        }

        public static IServiceCollection AddEFLocalization<TKey, TContext>(this IServiceCollection self)
            where TKey:IEquatable<TKey>
            where TContext : class, ILocalizationDbContext<TKey>
        {
            return self
                .AddScoped<ILocalizationDbContext<TKey>, TContext>()
                .AddSingleton<ILocalizationStringCollection, EFCollection<TKey>>();
        }

        public static IServiceCollection AddCookieCulture(this IServiceCollection self, string cookieField = "ASPNET_LANG")
        {
            return self.AddScoped<IRequestCultureProvider>(x => new CookieRequestCultureProvider(x.GetService<IHttpContextAccessor>(), cookieField));
        }

        public static IServiceCollection AddQueryCulture(this IServiceCollection self, string queryField = "lang")
        {
            return self.AddScoped<IRequestCultureProvider>(x => new QueryStringRequestCultureProvider(x.GetService<IHttpContextAccessor>(), queryField));
        }

        public static IServiceCollection AddRouteCulture(this IServiceCollection self, string routeField = "lang")
        {
            return self.AddScoped<IRequestCultureProvider>(x => new RouteRequestCultureProvider(x.GetService<IActionContextAccessor>(), routeField));
        }
    }
}
