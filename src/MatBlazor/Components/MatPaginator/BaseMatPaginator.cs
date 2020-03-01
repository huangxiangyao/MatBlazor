﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MatBlazor
{
    public class BaseMatPaginator : BaseMatDomComponent
    {
        private int _pageSize;

        [Parameter]
        public EventCallback<MatPaginatorPageEvent> Page { get; set; }

        [Parameter]
        public string Label { get; set; } = "Items per Page:";

        [Parameter]
        public string PageLabel { get; set; } = "Page:";


        [Parameter]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value;
        }


        [Parameter]
        public int Length { get; set; }

        public int PageIndex { get; set; }

        protected int TotalPages { get; set; }


        [Parameter]
        public EventCallback<int> PageIndexChanged { get; set; }


        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Update();
        }


        public void Update()
        {
            // Length = ParentDataTable?.ItemsComponent?.Length() ?? Length;
            TotalPages = CalculateTotalPages(PageSize);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            // if (ParentDataTable != null)
            // {
                // ParentDataTable.PaginatorComponent = this;
            // }

            this.Update();
        }

        protected int CalculateTotalPages(int pageSize)
        {
            if (pageSize == 0)
            {
                return int.MaxValue;
            }

            return Math.Max(0, (int) Math.Ceiling((decimal) Length / PageSize));
        }


        public async Task NavigateToPage(MatPaginatorAction direction, int pageSize)
        {
            var pageSizeChanged = pageSize != PageSize;
            var totalPages = CalculateTotalPages(pageSize);
            var page = PageIndex;

            if (pageSizeChanged)
            {
                try
                {
                    page = ((PageIndex * PageSize) / pageSize);
                }
                catch (OverflowException e)
                {
                }
            }

            try
            {
                checked
                {
                    switch (direction)
                    {
                        case MatPaginatorAction.Default:
                            break;
                        case MatPaginatorAction.First:
                            page = 0;
                            break;
                        case MatPaginatorAction.Previous:
                            page--;
                            break;
                        case MatPaginatorAction.Next:
                            page++;
                            break;
                        case MatPaginatorAction.Last:
                            page = totalPages;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                    }
                }
            }
            catch (Exception e)
            {
            }

            if (page < 0)
            {
                page = 0;
            }

            if (totalPages - page <= 1)
            {
                page = TotalPages == 0 ? 0 : TotalPages - 1;
            }

            if (PageIndex != page || pageSize != PageSize)
            {
                PageIndex = page;
                PageSize = pageSize;
                await Page.InvokeAsync(new MatPaginatorPageEvent()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    Length = Length,
                });
                // ParentDataTable?.Update();
            }
        }


        [Parameter]
        public IReadOnlyList<MatPageSizeOption> PageSizeOptions { get; set; } = new[]
        {
            new MatPageSizeOption(5),
            new MatPageSizeOption(10),
            new MatPageSizeOption(25),
            new MatPageSizeOption(50),
            new MatPageSizeOption(100),
            new MatPageSizeOption(int.MaxValue, "*"),
        };


        protected async Task PageSizeChangedHandler(int value)
        {
            await NavigateToPage(MatPaginatorAction.Default, value);
        }
    }
}