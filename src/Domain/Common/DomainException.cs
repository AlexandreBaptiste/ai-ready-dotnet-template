namespace Domain.Common;

/// <summary>
/// Base exception for all domain rule violations.
/// Throw this (or a subclass) when a business rule is broken.
/// </summary>
public class DomainException(string message) : Exception(message);
