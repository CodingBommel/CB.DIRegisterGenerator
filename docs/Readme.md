# Coding Bommel - Dependency Injection Register Generator

Coding Bommel provides you an code gernerator analyser that generate for your assembly an IServiceCollection extension methode that register all classes they implement an interface transiently.

if you don't want to register a interface transiently to ServiceCollection you can skip it with the attribute [IgnoreForDIRegisterAttribute].
