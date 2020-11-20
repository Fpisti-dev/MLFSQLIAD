<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MLFSQLIAD._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class="jumbotron p-4 p-md-5 text-white rounded bg-dark" style="margin-top: 4em">
        <div class="col-md-8 px-0">
            <h1 class="display-4 font-italic">ML.NET</h1>
            <p class="lead my-2">How to use machine learning to protect against malicious attacks using the SQL Injection method in server-side defence in the ASP.NET framework?</p>
        </div>
     </div>

    <div class="col-sm-12 row">
  
        <div class="input-group col-sm-8">

            <asp:TextBox ID="txtInput" runat="server" class="form-control" placeholder="SQL Command"></asp:TextBox>

            <div class="input-group-append">
           
                <asp:Button ID="btnSubmit" class="btn btn-outline-secondary" runat="server" Text="Submit" OnClick="btnSubmit_Click" />   
            
            </div>
        </div>

        <div class="input-group col-sm-4"></div>

    </div>
    <br />

    <div class="col-sm-12">

        <asp:TextBox class="form-control" ID="txtResult" runat="server" Rows="20" Columns="80" TextMode="MultiLine"></asp:TextBox>

    </div>

    <script type="text/javascript" src="Default.js"></script> 

</asp:Content>
