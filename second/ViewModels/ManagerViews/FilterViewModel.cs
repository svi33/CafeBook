using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels
{
    public class FilterViewModel
    {
        public FilterViewModel(List<HallSimple> halls, string hall, DateTime date)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            halls.Insert(0, new HallSimple { HallName = "Все", HallID = "0" });
            Halls = new SelectList(halls, "HallID", "HallName", hall);
            SelectedHall = hall;
            SelectedDate = date;
        }
        public SelectList Halls { get; private set; }  
        public string SelectedHall { get; private set; }    
        public DateTime SelectedDate { get; private set; }    
    }
}
