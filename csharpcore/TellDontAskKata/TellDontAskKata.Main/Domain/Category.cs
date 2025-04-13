﻿namespace TellDontAskKata.Main.Domain
{
    public class Category
    {
        public Category(string name, decimal taxPercentage)
        {
            Name = name;
            TaxPercentage = taxPercentage;
        }

        public string Name { get; set; }
        public decimal TaxPercentage { get; set; }
        
        
    }
}
