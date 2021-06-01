/***********************************************************************
 *            Project: CoreCms.Net                                     *
 *                Web: https://CoreCms.Net                             *
 *        ProjectName: 核心内容管理系统                                *
 *             Author: 大灰灰                                          *
 *              Email: JianWeie@163.com                                *
 *         CreateTime: 2020-05-08 23:27:26
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using CoreCms.Net.Caching.AutoMate.MemoryCache;
using CoreCms.Net.Core.Attribute;

namespace CoreCms.Net.Core.AOP
{
    /// <summary>
    /// 面向切换的内存缓存使用
    /// </summary>
    public class MemoryCacheAop : CacheAopBase
    {
        //通过注入的方式，把缓存操作接口通过构造函数注入
        private readonly ICachingProvider _cache;
        public MemoryCacheAop(ICachingProvider cache)
        {
            _cache = cache;
        }

        //Intercept方法是拦截的关键所在，也是IInterceptor接口中的唯一定义
        public override void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            var CachingAttribute = method.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(CachingAttribute));
            if (CachingAttribute is CachingAttribute qCachingAttribute)
            {
                //获取自定义缓存键
                var cacheKey = CustomCacheKey(invocation);
                //根据key获取相应的缓存值
                var cacheValue = _cache.Get(cacheKey);
                if (cacheValue != null)
                {
                    //将当前获取到的缓存值，赋值给当前执行方法
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                //去执行当前的方法
                invocation.Proceed();
                //存入缓存
                if (!string.IsNullOrWhiteSpace(cacheKey))
                {
                    _cache.Set(cacheKey, invocation.ReturnValue, qCachingAttribute.AbsoluteExpiration);
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }

    }
}
