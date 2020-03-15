﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:DocumentStore.Application.CommandHandlers.DeleteDocumentCommandHandler.Handle(DocumentStore.Application.Command.DeleteDocumentCommand,System.Threading.CancellationToken)~System.Threading.Tasks.Task{MediatR.Unit}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:DocumentStore.Application.CommandHandlers.BaseCommandHandlerWithValidation`1.Handle(`0,System.Threading.CancellationToken)~System.Threading.Tasks.Task{MediatR.Unit}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:DocumentStore.Application.CommandHandlers.UploadDocumentCommandHandler.ProcessHandle(DocumentStore.Application.Command.UploadDocumentCommand,System.Threading.CancellationToken)~System.Threading.Tasks.Task{MediatR.Unit}")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:DocumentStore.Application.QueryHandlers.GetDocumentQueryHandler.Handle(DocumentStore.Application.Query.DownloadDocumentQuery,System.Threading.CancellationToken)~System.Threading.Tasks.Task{DocumentStore.Application.Dtos.DocumentDto}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:DocumentStore.Application.QueryHandlers.GetDocumentsQueryHandler.Handle(DocumentStore.Application.Query.GetDocumentsQuery,System.Threading.CancellationToken)~System.Threading.Tasks.Task{System.Collections.Generic.List{DocumentStore.Application.Dtos.DocumentInfoDto}}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>", Scope = "member", Target = "~M:DocumentStore.Application.RequestHandlerBehaviorWithCircuitBreaker`2.Handle(`0,System.Threading.CancellationToken,MediatR.RequestHandlerDelegate{`1})~System.Threading.Tasks.Task{`1}")]