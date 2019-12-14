﻿using Microsoft.AspNetCore.Components;

namespace MatBlazor
{
    public class BaseMatRadioGroup : BaseMatComponent
    {
        private string _value;

        [Parameter]
        public RenderFragment ChildContent { get; set; }


        [Parameter]
        public string Id { get; set; } = IdGeneratorHelper.Generate("");


        [Parameter]
        public string Value
        {
            get => _value;
            set
            {
                if (value != _value)
                {
                    _value = value;
                    ValueChanged.InvokeAsync(value);
                }
            }
        }


        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }
    }
}