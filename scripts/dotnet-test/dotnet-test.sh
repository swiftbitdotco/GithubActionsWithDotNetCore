#!/bin/bash
dotnet test \
--no-build \
-l "trx;verbosity=detailed;logfilename=TestResults.trx" \
-l "console;verbosity=detailed" \
-l "html;verbosity=detailed;logfilename=TestResults.html" \
--results-directory="TestResults" \
--collect="XPlat Code Coverage" \
./test/GithubActions.Tests/GithubActions.Tests.csproj