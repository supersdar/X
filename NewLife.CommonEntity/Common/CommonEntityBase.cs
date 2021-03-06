﻿using System;
using System.Text;
using XCode;

namespace NewLife.CommonEntity
{
    /// <summary>通用实体类基类</summary>
    /// <typeparam name="TEntity"></typeparam>
    [Serializable]
    public class CommonEntityBase<TEntity> : Entity<TEntity> where TEntity : CommonEntityBase<TEntity>, new()
    {
        #region 改动时写日志
        /// <summary>已重载。调用Save时写日志，而调用Insert和Update时不写日志</summary>
        /// <returns></returns>
        public override int Save()
        {
            if ((this as IEntity).IsNullKey)
                WriteLog("添加", this);
            else
            {
                // 没有修改时不写日志
                if (!HasDirty) return 0;

                WriteLog("修改", this);
            }

            return base.Save();
        }
        #endregion

        #region 日志
        /// <summary>写日志</summary>
        /// <param name="action">操作</param>
        /// <param name="remark">备注</param>
        public static void WriteLog(String action, String remark)
        {
            ManageProvider.Provider.WriteLog(typeof(TEntity), action, remark);
        }

        /// <summary>输出实体对象日志</summary>
        /// <param name="action"></param>
        /// <param name="entity"></param>
        protected static void WriteLog(String action, IEntity entity)
        {
            // 构造字段数据的字符串表示形式
            var sb = new StringBuilder();
            foreach (var fi in Meta.Fields)
            {
                if (action == "修改" && !entity.Dirtys[fi.Name]) continue;

                sb.Separate(",").AppendFormat("{0}={1}", fi.Name, entity[fi.Name]);
            }

            WriteLog(action, sb.ToString());
        }
        #endregion
    }
}