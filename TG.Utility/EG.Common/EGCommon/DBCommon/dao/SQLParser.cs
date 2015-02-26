using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EG.Utility.DBCommon.dao
{
    public class SQLParser : Object2SQL
    {

	    private const char PROPERTY_START_FLAG = '[' ;
        private const char PROPERTY_END_FLAG = ']';

        private const char SEGMENT_START_FLAG = '{';
        private const char SEGMENT_END_FLAG = '}';

        private const char REPLACE_START_FLAG = '<';
	    private const char REPLACE_END_FLAG = '>' ;
	

	    private Object bean ;
        private Hashtable mapBean ;
	
	    private String input ;
	
	    private StringBuilder output = null ;
	
	    private int current = -1 ;
	    private int last ;
	    private char c ;

        private char ParamFlag = '@';

        public SQLParser()
        {
            TransactionContext context = TransactionContext.get() ;
            if (context == null) 
            {
                return;
            }

            if (context.dbType == ADOTemplate.DB_TYPE_ORACLE) 
            {
                ParamFlag = ':';
            }
	    }

        public SQLParser(char pParamFlag)
        {
            this.ParamFlag = pParamFlag;
        }

        public String Parse(Object bean) {
            throw new Exception("not support");
        }

        public String Parse(String input, Object bean)
        {
            this.output = new StringBuilder();
            this.input = input;

            this.current = -1;
            this.last = input.Length;

            this.mapBean = bean as Hashtable;
            if (this.mapBean == null)
            {
                this.bean = bean;
            }

            this.ParameterNames = new List<String>();
            this.ParameterValues = new List<Object>();

		    while (nextChar()) {			
			    if (c == PROPERTY_START_FLAG) {
				    propertyStart() ;
			    } else if (c == SEGMENT_START_FLAG){
				    segmentStart() ;
			    } else if (c == REPLACE_START_FLAG && input[current + 1] == REPLACE_START_FLAG){
				    // "<<"  end of replace is ">>"
				    replaceStart() ;
			    } else {
				    output.Append(c) ;
			    }
		    }
		
		    return output.ToString() ;
	    }
	
	
	    private bool nextChar() {

		    current ++ ;
		
		    if (current < last) {
			    c = input[current] ;
			    return true ;
		    } else {
			    return false ;
		    }
	    }
	
	    private void replaceStart() {

		    int start = current + 2;
		
		    while (nextChar()) {			
			    if (c == REPLACE_END_FLAG) {
                    String perporty = input.Substring(start, current - start);
				
				    Object value = null ;
				
				    try {
					    value =
						    mapBean == null ? PropertyUtils.GetValue(bean, perporty)
							    : mapBean[perporty];
				    } catch (Exception e) {
					    throw e ;
				    }

				    output.Append(value) ;
				
				    current ++ ;// next char is '>', so skip it !
				
				    return ;
			    } 
		    }
	    }		
	

	    private void propertyStart() {

		    int start = current + 1;
		
		    while (nextChar()) {			
			    if (c == PROPERTY_END_FLAG) {
                    String perporty = input.Substring(start, current - start);
				
				    Object value = null ;
				
				    //try {
					    value =
						    mapBean == null ? PropertyUtils.GetValue(bean, perporty)
							    : mapBean[perporty];
					    // try to find 'like' operator
				        if (value != null) {
						    int offset = -2 ;
						    char curChar = input[start + offset] ;
						    while (curChar == ' ') {
							    offset -- ;
							    curChar = input[start + offset] ;
						    }
						
						    if ((curChar == 'e' || curChar == 'E') && value.ToString().IndexOf('%') < 0) {
							    if (C.EMPTY_STRING.Equals(value)) {// like '' => not to generate where_party 
								    value = null ;
							    } else {// add % to value
							        value = "%" + value + "%" ;
							    }
						    }
				        }
				    //} catch (Exception e) {
					//    throw e ;
				    //}
 
				    if (value is Object[]) {
					    bool first = true ;
                        Object[] objArray = (value as Object[]) ;
					    for(int i = 0 ; i < objArray.Length ; i ++) {
                            Object obj = objArray[i];
						    if (obj == null) {
							    continue ;
						    }
						    if (first) {
							    first = false ;
						    } else {
							    output.Append(',') ;
							    output.Append(' ') ;
						    }
                            //params_.Add(obj);
                            this.ParameterNames.Add(perporty + i);
                            this.ParameterValues.Add(obj);
                            output.Append(ParamFlag + perporty + i);
					    }

                        if (first)// 没有参数，需要加一个参数，以作空参数回退之用
                        {
                            //params_.Add(null);
                            this.ParameterNames.Add(perporty);
                            this.ParameterValues.Add(null);
                            output.Append(ParamFlag + perporty);
					    }
				    } else if (value is ICollection<Object>) {
                        //IEnumerable<Object> l = ((ICollection<Object>)value).AsEnumerable<Object>();
                        ICollection<Object> valueCollection = value as ICollection<Object>;
					    bool first = true ;
                        int i = 0;
                        foreach (Object elemValue in valueCollection)
                        {
						    if (first) {
							    first = false ;
						    } else {
							    output.Append(',') ;
							    output.Append(' ') ;
						    }
                            //params_.Add(elemValue);
                            this.ParameterNames.Add(perporty + (i));
                            this.ParameterValues.Add(elemValue);
                            output.Append(ParamFlag + perporty + (i++));
					    }
				    } else {
                        //params_.Add(value);
                        this.ParameterNames.Add(perporty);
                        this.ParameterValues.Add(value);
                        output.Append(ParamFlag + perporty);					
				    }
				    return ;
			    } 
		    }
	    }


	    private void segmentStart() {

		    //int start = current + 1;
		    int paramsStart = this.ParameterValues.Count ;
		
		    StringBuilder temp = output ;
		
		    output = new StringBuilder() ;
		
		    while (nextChar()) {			
			    if (c == PROPERTY_START_FLAG) {
				    propertyStart() ;
			    } else if (c == SEGMENT_END_FLAG) {

                    Object lastParam = this.ParameterValues[this.ParameterValues.Count - 1];
				
				    if (lastParam == null) {
                        for (int last = this.ParameterValues.Count - 1; last >= paramsStart; last--)
                        {
                            this.ParameterValues.RemoveAt(last);
                            this.ParameterNames.RemoveAt(last);
					    }
					
					    output = temp ;
				    } else {
					    temp.Append(output) ;
				        output = temp ;
				    }
				    return ;
			    } else {
				    output.Append(c) ;
			    }
		    }
	    }

        public String AsSql()
        {
            return output.ToString();
        }

        public String[] GetSqlParameterNames()
        {
            return this.ParameterNames.ToArray<String>();
        }

        public Object[] GetSqlParameterValues()
        {
            return this.ParameterValues.ToArray<Object>();
        }
    }

    public class PropertyUtils
    {
        public static object GetValue(Object obj, String property) {
            return obj.GetType().GetProperty(property).GetValue(obj, null);
        }
    }

    public class C
    { 
        public const string EMPTY_STRING = "" ;
    }
}
