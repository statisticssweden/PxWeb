using System;
using System.Collections.Generic;
using System.Text;


namespace PCAxis.Sql.Pxs {
    /// <summary>
    /// This class is used be the FlatFileReader-method to store some temp-values.
    /// </summary>
    class FlatFileReaderHelper{
     
        internal int no_vars = -1;
        internal int no_vars_before_heading = -1;
        internal int no_vars_before_time = -1;
        internal int var_counter = -1;
        internal String langInFile = null;



        //The [] determin which variable ( VAR0, VAR1 ...)
        // the list conatins the values for a variable

        internal PQVariable[] tmpVariable;
        internal List<ValueTypeWithGroup>[] variableValues;

        
        //The [] determin which variable ( VAR0, VAR1 ...)
        // the dictionary use the molecule code as key for list of atom codes 
        internal Dictionary<string, List<GroupValueType>>[] groupsValuesByValueCode;

        //The [] determin which variable ( VAR0, VAR1 ...)
        // the dictionary use the value sortorder as key for text 
        internal Dictionary<int, string>[] groupTextByValueSortOrder;

        internal List<BasicValueType> timeValues = new List<BasicValueType>();
        internal List<BasicValueType> contentValues = new List<BasicValueType>();
        internal List<MenuSelType> menuSelList = new List<MenuSelType>();



        internal FlatFileReaderHelper() { }


      
    }
}
