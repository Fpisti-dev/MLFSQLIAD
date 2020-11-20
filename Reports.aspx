<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MLFSQLIAD.Reports" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %> Page</h2>
    
    <br /><br />
    
    <ul class="nav nav-tabs" id="myTab" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="charts-tab" data-toggle="tab" href="#charts" role="tab" aria-controls="charts" aria-selected="true">Charts</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="test-tab" data-toggle="tab" href="#test" role="tab" aria-controls="test" aria-selected="false">Test History</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="url-tab" data-toggle="tab" href="#url" role="tab" aria-controls="url" aria-selected="false">URL History</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="input-tab" data-toggle="tab" href="#input" role="tab" aria-controls="input" aria-selected="false">Input History</a>
        </li>
    </ul>

    <div class="tab-content" id="myTabContent">

        <div class="tab-pane fade show active" id="charts" role="tabpanel" aria-labelledby="charts-tab">
            <br />
            <div class="col-sm-12 row">

                <div class="form-group col-sm-6 row">

                    <asp:Label class="col-sm-4 control-label" for="txtTestNumber" ID="lblTestNumber" runat="server" Text="Test Number"></asp:Label> 
                    <div class="col-sm-5">
                        <asp:TextBox class="form-control" ID="txtTestNumber" runat="server" type="number"></asp:TextBox>
                        <asp:rangevalidator ID="RangeValidator1" errormessage="Out of range !" forecolor="Red" controltovalidate="txtTestNumber" minimumvalue="1" maximumvalue="99" runat="server" Type="Integer">
                        </asp:rangevalidator>
                    </div>            
                    
                </div>

                <div class="col-sm-6">
                    <asp:DropDownList class="form-control col-sm-10" ID="ddlCharType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCharType_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>

            <br />
            <div class="col-sm-12">
                <asp:Label class="control-label" ID="lblInfo" runat="server" Text=""></asp:Label>
                <hr />
            </div>


            <div class="col-sm-12 row">

                <div class="col-sm-6">
                    <asp:Literal ID="ltChart" runat="server"></asp:Literal>
                    <asp:Panel ID="pnlChart" runat="server"></asp:Panel>
                    <div id="chart_div"></div> 
                </div>

                <div class="col-sm-6">
                    <asp:Literal ID="ltMetrics" runat="server"></asp:Literal>
                    <asp:Panel ID="pnlMetrics" runat="server"></asp:Panel>
                    <div id="metrics_div"></div> 
                </div>

                
            </div>
             
        </div>  

        <div class="tab-pane fade" id="test" role="tabpanel" aria-labelledby="test-tab">
            <asp:Panel ID="pnlTest" runat="server" style="margin-top:3em"></asp:Panel>
        </div>

        <div class="tab-pane fade" id="url" role="tabpanel" aria-labelledby="url-tab">
            <asp:Panel ID="pnlURL" runat="server" style="margin-top:3em"></asp:Panel>
        </div>

        <div class="tab-pane fade" id="input" role="tabpanel" aria-labelledby="input-tab">
            <asp:Panel ID="pnlInput" runat="server" style="margin-top:3em"></asp:Panel>
        </div>
        
    </div>

    <script type="text/javascript" src="Reports.js"></script> 

</asp:Content>

