﻿// 框架名称：Fur
// 框架作者：百小僧
// 框架版本：1.0.0
// 开源协议：MIT
// 项目地址：https://gitee.com/monksoul/Fur

using Fur;
using Fur.Configurable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurableServiceCollectionExtensions
    {
        /// <summary>
        /// 添加选项配置
        /// </summary>
        /// <typeparam name="TOptions">选项类型</typeparam>
        /// <param name="services">服务集合</param>
        /// <param name="options">选项实例</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddAppOptions<TOptions>(this IServiceCollection services)
            where TOptions : class, IAppOptions
        {
            var optionsType = typeof(TOptions);
            var optionsSettings = optionsType.GetCustomAttribute<OptionsSettingsAttribute>(false);

            // 获取键名
            string jsonKey = GetOptionsJsonKey(optionsSettings, optionsType);

            // 配置选项（含验证信息）
            var optionsConfiguration = App.Configuration.GetSection(jsonKey);

            // 配置选项监听
            if (typeof(IAppOptionsListener<TOptions>).IsAssignableFrom(optionsType))
            {
                var onListenerMethod = optionsType.GetMethod(nameof(IAppOptionsListener<TOptions>.OnListener));
                if (onListenerMethod != null)
                {
                    ChangeToken.OnChange(() => optionsConfiguration.GetReloadToken(), () =>
                    {
                        var options = optionsConfiguration.Get<TOptions>();
                        onListenerMethod.Invoke(options, new object[] { options, optionsConfiguration });
                    });
                }
            }

            services.AddOptions<TOptions>()
                .Bind(optionsConfiguration)
                .ValidateDataAnnotations();

            // 配置复杂验证后后期配置
            var validateInterface = optionsType.GetInterfaces()
                .FirstOrDefault(u => u.IsGenericType && typeof(IAppOptions).IsAssignableFrom(u.GetGenericTypeDefinition()));
            if (validateInterface != null)
            {
                var genericArguments = validateInterface.GenericTypeArguments;

                // 配置复杂验证
                if (genericArguments.Length > 1)
                {
                    services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IValidateOptions<TOptions>), genericArguments.Last()));
                }

                // 配置后期配置
                var postConfigureMethod = optionsType.GetMethod(nameof(IAppOptions<TOptions>.PostConfigure));
                if (postConfigureMethod != null)
                {
                    if (optionsSettings?.PostConfigureAll != true)
                        services.PostConfigure<TOptions>(options => postConfigureMethod.Invoke(options, new object[] { options, optionsConfiguration }));
                    else
                        services.PostConfigureAll<TOptions>(options => postConfigureMethod.Invoke(options, new object[] { options, optionsConfiguration }));
                }
            }

            return services;
        }

        /// <summary>
        /// 获取选项键
        /// </summary>
        /// <param name="optionsSettings">选项配置特性</param>
        /// <param name="optionsType">选项类型</param>
        /// <returns></returns>
        private static string GetOptionsJsonKey(OptionsSettingsAttribute optionsSettings, Type optionsType)
        {
            // 默认后缀
            var defaultStuffx = nameof(Options);

            return optionsSettings switch
            {
                // // 没有贴 [OptionsSettings]，如果选项类以 `Options` 结尾，则移除，否则返回类名称
                null => optionsType.Name.EndsWith(defaultStuffx) ? optionsType.Name[0..^defaultStuffx.Length] : optionsType.Name,
                // 如果贴有 [OptionsSettings] 特性，但未指定 JsonKey 参数，则直接返回类名，否则返回 JsonKey
                _ => optionsSettings != null && string.IsNullOrEmpty(optionsSettings.JsonKey) ? optionsType.Name : optionsSettings.JsonKey,
            };
        }
    }
}