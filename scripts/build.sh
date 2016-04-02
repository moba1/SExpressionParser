#!/bin/sh

xbuild /p:Configuration=Release src/SExpressionParser.sln /p:TargetFrameworkVersion="v4.5" /p:DebugSymbols=False

