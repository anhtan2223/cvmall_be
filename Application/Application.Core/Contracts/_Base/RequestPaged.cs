﻿using Framework.Core.Abstractions;

namespace Application.Core.Contracts
{
    public class RequestPaged : IRequestPaged
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? search { get; set; }

        public string? filter { get; set; }

    }
}
