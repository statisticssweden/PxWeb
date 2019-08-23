using System;
using System.Runtime.Serialization;

namespace PCAxis.Sql.Exceptions 
{

   ///<summary>
   /// Parent exception for common catching.
   ///</summary>
   [Serializable]
   public abstract class PCAxisSqlException : ApplicationException{
            public PCAxisSqlException() : base() { }
            public PCAxisSqlException(String message) : base(message) { }
            public PCAxisSqlException(String message, Exception innerException) : base(message, innerException) { }
            public PCAxisSqlException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
   }
 


   ///<summary>
   /// Something wrong in database, pxs or both
   ///</summary>
   [Serializable]
   public class DbPxsMismatchException : PCAxisSqlException{
            public DbPxsMismatchException() : base() { }
            public DbPxsMismatchException(String message) : base(message) { }
            public DbPxsMismatchException(String message, Exception innerException) : base(message, innerException) { }
            public DbPxsMismatchException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public DbPxsMismatchException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public DbPxsMismatchException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public DbPxsMismatchException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public DbPxsMismatchException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
   }
 


   ///<summary>
   /// Something wrong in Config , pxs or both
   ///</summary>
   [Serializable]
   public class ConfigPxsMismatchException : PCAxisSqlException{
            public ConfigPxsMismatchException() : base() { }
            public ConfigPxsMismatchException(String message) : base(message) { }
            public ConfigPxsMismatchException(String message, Exception innerException) : base(message, innerException) { }
            public ConfigPxsMismatchException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public ConfigPxsMismatchException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public ConfigPxsMismatchException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public ConfigPxsMismatchException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public ConfigPxsMismatchException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
   }
 


   ///<summary>
   /// Something wrong in database, Config  or both
   ///</summary>
   [Serializable]
   public class ConfigDbMismatchException : PCAxisSqlException{
            public ConfigDbMismatchException() : base() { }
            public ConfigDbMismatchException(String message) : base(message) { }
            public ConfigDbMismatchException(String message, Exception innerException) : base(message, innerException) { }
            public ConfigDbMismatchException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public ConfigDbMismatchException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public ConfigDbMismatchException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public ConfigDbMismatchException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public ConfigDbMismatchException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
   }
 


   ///<summary>
   /// Something wrong in the database
   ///</summary>
   [Serializable]
   public class DbException : PCAxisSqlException{
            public DbException() : base() { }
            public DbException(String message) : base(message) { }
            public DbException(String message, Exception innerException) : base(message, innerException) { }
            public DbException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public DbException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public DbException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public DbException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public DbException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
            public DbException(int ErrNo, Object arg0, Object arg1, Object arg2, Object arg3) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2, arg3).getText()) { }

   }
 


   ///<summary>
   /// Something wrong in the Config file
   ///</summary>
   [Serializable]
   public class ConfigException : PCAxisSqlException{
            public ConfigException() : base() { }
            public ConfigException(String message) : base(message) { }
            public ConfigException(String message, Exception innerException) : base(message, innerException) { }
            public ConfigException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public ConfigException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public ConfigException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public ConfigException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public ConfigException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
            public ConfigException(int ErrNo, Object arg0, Object arg1, Object arg2, Object arg3) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2, arg3).getText()) { }
   }
 


   ///<summary>
   /// Something wrong in the PXS-file
   ///</summary>
   [Serializable]
   public class PxsException : PCAxisSqlException{
            public PxsException() : base() { }
            public PxsException(String message) : base(message) { }
            public PxsException(String message, Exception innerException) : base(message, innerException) { }
            public PxsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public PxsException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public PxsException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public PxsException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public PxsException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
   }
 


   ///<summary>
   /// Bug in application
   ///</summary>
   [Serializable]
   public class BugException : PCAxisSqlException{
            public BugException() : base() { }
            public BugException(String message) : base(message) { }
            public BugException(String message, Exception innerException) : base(message, innerException) { }
            public BugException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
            public BugException(int ErrNo) : base(new ErrNo2Text(ErrNo).getText()) { }
            public BugException(int ErrNo, Object arg0) : base(new ErrNo2Text(ErrNo,arg0).getText()) { }
            public BugException(int ErrNo, Object arg0, Object arg1) : base(new ErrNo2Text(ErrNo, arg0, arg1).getText()) { }
            public BugException(int ErrNo, Object arg0, Object arg1, Object arg2) : base(new ErrNo2Text(ErrNo, arg0, arg1, arg2).getText()) { }
   }
 


}
