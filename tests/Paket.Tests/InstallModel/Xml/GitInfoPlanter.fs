﻿module Paket.InstallModel.Xml.GitInfoPlanterSpecs

open Paket
open NUnit.Framework
open FsUnit
open Paket.TestHelpers
open Paket.Domain
open Paket.Requirements

let emptyReferences = """<?xml version="1.0" encoding="utf-16"?>
<Choose xmlns="http://schemas.microsoft.com/developer/msbuild/2003" />"""

let emptyPropertyDefinitionNodes = """<?xml version="1.0" encoding="utf-16"?>
<Choose xmlns="http://schemas.microsoft.com/developer/msbuild/2003" />"""

let expectedPropertyNodes = """<?xml version="1.0" encoding="utf-16"?>
<Import Project="..\..\..\GitInfoPlanter\build\GitInfoPlanter.targets" Condition="Exists('..\..\..\GitInfoPlanter\build\GitInfoPlanter.targets')" Label="Paket" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" />"""

[<Test>]
let ``should generate Xml for GitInfoPlanter2.0.0``() = 
    let model =
        InstallModel.CreateFromLibs(PackageName "GitInfoPlanter", SemVer.Parse "0.21", [],
            [ ],
            [ @"..\GitInfoPlanter\build\GitInfoPlanter.targets" ],
            [],
              Nuspec.All)

    let propsNodes,targetsNodes,chooseNode,propertyChooseNode,_ = ProjectFile.TryLoad("./ProjectFile/TestData/Empty.fsprojtest").Value.GenerateXml(model, System.Collections.Generic.HashSet<_>(),Map.empty,Some true,true,None)
    chooseNode.Head.OuterXml
    |> normalizeXml
    |> shouldEqual (normalizeXml emptyReferences)

    propertyChooseNode.OuterXml
    |> normalizeXml
    |> shouldEqual (normalizeXml emptyPropertyDefinitionNodes)

    propsNodes |> Seq.length |> shouldEqual 0
    targetsNodes |> Seq.length |> shouldEqual 1

    (targetsNodes |> Seq.head).OuterXml
    |> normalizeXml
    |> shouldEqual (normalizeXml expectedPropertyNodes)