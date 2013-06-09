﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;

namespace ShubhaRt
{
    



        [DelimitedRecord(","), IgnoreFirst(1), IgnoreEmptyLines(true)]
        public class Option
        {
            public string SECURITY;
            [FieldOptional()]
            public string SYMBOL;
            [FieldOptional()]
            public string EXP_DATE;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

          
            public double STR_PRICE;

            [FieldNullValue(typeof(string ), "0")]
            [FieldOptional()]
            public string OPT_TYPE;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double OPEN_PRICE;
            [FieldNullValue(typeof(double), "0")]
            [FieldOptional()]
            public double HIGH_PRICE;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double LOW_PRICE;
            [FieldNullValue(typeof(double), "0")]

            [FieldOptional()]

            public double CLOSE_PRICE;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double OPEN_INT;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double TRD_VAL;
            [FieldNullValue(typeof(int), "0")]
            [FieldOptional()]

            public int NET_TRDQTY;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double NO_OF_CONT;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double NO_OF_TRADE;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double NOTION_VAL;
            [FieldOptional()]
            [FieldNullValue(typeof(double), "0")]

            public double PR_VAL;


        }

        [DelimitedRecord(","), IgnoreFirst(1)]
        public class OptionFINAL
        {
            public string ticker;
            public string name;
            public string date;
            public double open;
            public double high;
            public double low;
            public double close;
            public int volume;
            [FieldNullValue(typeof(long), "0")]
            public Nullable<long> openint;
        }



    
}
