using second.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace second.ViewModels
{
    public class SortViewModel
    {
        public SortState CheckInSort { get; private set; } // значение для сортировки по имени
        public SortState HallSort { get; private set; }    // значение для сортировки по возрасту
        public SortState PhoneSort { get; private set; }   // значение для сортировки по компании
        public SortState Current { get; private set; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            CheckInSort = sortOrder == SortState.CheckInAsc? SortState.CheckInDesc : SortState.CheckInAsc;
            HallSort = sortOrder == SortState.HallAsc? SortState.HallDesc : SortState.HallAsc;
            PhoneSort = sortOrder == SortState.PhoneAsc? SortState.PhoneDesc : SortState.PhoneAsc;
            Current = sortOrder;
        }
            
}
}
