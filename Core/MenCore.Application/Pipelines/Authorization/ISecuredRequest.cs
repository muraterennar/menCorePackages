﻿namespace MenCore.Application.Pipelines.Authorization;

public interface ISecuredRequest
{
    public string[] Roles { get; }
}