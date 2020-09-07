﻿// -----------------------------------------------------------------------------
// Fur 是 .NET 5 平台下极易入门、极速开发的 Web 应用框架。
// Copyright © 2020 Fur, Baiqian Co.,Ltd.
//
// 框架名称：Fur
// 框架作者：百小僧
// 框架版本：1.0.0
// 源码地址：https://gitee.com/monksoul/Fur
// 开源协议：Apache-2.0（http://www.apache.org/licenses/LICENSE-2.0）
// -----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fur.DatabaseAccessor
{
    /// <summary>
    /// Sql 查询仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface ISqlQueryableRepository<TEntity> : ISqlQueryableRepository<TEntity, DbContextLocator>
        where TEntity : class, IEntityBase, new()
    {
    }

    /// <summary>
    /// Sql 查询仓储接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TDbContextLocator">数据库实体定位器</typeparam>
    public interface ISqlQueryableRepository<TEntity, TDbContextLocator> : IRepositoryDependency
        where TEntity : class, IEntityBase, new()
        where TDbContextLocator : class, IDbContextLocator, new()
    {
        /// <summary>
        /// 执行 Sql 返回 IQueryable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>IQueryable<TEntity></returns>
        IQueryable<TEntity> FromSqlRaw(string sql, params object[] parameters);

        /// <summary>
        /// 执行 Sql 返回 IQueryable
        /// </summary>
        /// <remarks>
        /// 支持字符串内插语法
        /// </remarks>
        /// <param name="sql">sql 语句</param>
        /// <returns>IQueryable<TEntity></returns>
        IQueryable<TEntity> FromSqlInterpolated(FormattableString sql);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataTable</returns>
        DataTable SqlQuery(string sql, params object[] parameters);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>DataTable</returns>
        DataTable SqlQuery(string sql, object model);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task<DataTable></returns>
        Task<DataTable> SqlQueryAsync(string sql, params object[] parameters);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task<DataTable></returns>
        Task<DataTable> SqlQueryAsync(string sql, object[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sql 查询返回 DataTable
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task<DataTable></returns>
        Task<DataTable> SqlQueryAsync(string sql, object model, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>List<T></returns>
        List<T> SqlQuery<T>(string sql, params object[] parameters);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <returns>List<T></returns>
        List<T> SqlQuery<T>(string sql, object model);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> SqlQueryAsync<T>(string sql, params object[] parameters);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="parameters">命令参数</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> SqlQueryAsync<T>(string sql, object[] parameters, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sql 查询返回 List 集合
        /// </summary>
        /// <param name="sql">sql 语句</param>
        /// <param name="model">参数模型</param>
        /// <param name="cancellationToken">异步取消令牌</param>
        /// <returns>Task<List<T>></returns>
        Task<List<T>> SqlQueryAsync<T>(string sql, object model, CancellationToken cancellationToken = default);
    }
}