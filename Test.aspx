<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Test.aspx.cs" Inherits="MLFSQLIAD.Test" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <br />
    <h3>Test Page for MLFSQLIAD App</h3>

    <asp:HiddenField ID="hID" runat="server" />

    <br />
    <div class="col-sm-12 row">

        <div class="input-group col-sm-4"> 
            <h4>Training data source</h4>
        </div>

        <div class="input-group col-sm-2">
            <%--<asp:RadioButtonList ID="rdoDataSource" runat="server" ForeColor="#1D91CA" ValidationGroup="General" 
                AutoPostBack="True" RepeatDirection="Horizontal" CssClass="radioButtonList" OnSelectedIndexChanged="rdoDataSource_SelectedIndexChanged">
                <asp:ListItem Value="project">&nbsp;ProjectData</asp:ListItem>
                <asp:ListItem Value="burp_suite">&nbsp;BurpSuitee</asp:ListItem>
                <asp:ListItem Value="fuzzydb">&nbsp;FuzzyDB</asp:ListItem>
                <asp:ListItem Value="OWASP">&nbsp;OWASP</asp:ListItem>
            </asp:RadioButtonList>--%>
        </div>

        <div class="form-group col-sm-3 row">
            <asp:Label class="col-sm-7 control-label" for="txtTestNumber" ID="lblTestNumber" runat="server" Text="Test Number"></asp:Label> 
            <div class="col-sm-5">
                <asp:TextBox class="form-control" ID="txtTestNumber" runat="server" type="number"></asp:TextBox>
            </div>            
        </div>

        <div class="input-group col-sm-2">        
           
            <asp:Label class="col-sm-8 control-label" ID="lblLastNumber" runat="server" Text="Last Number" ForeColor="#CCCCCC" Font-Size="Small"></asp:Label>

        </div>

        <div class="col-sm-1">        
           
            <asp:Button ID="btnTest" class="btn btn-primary" runat="server" Text="Run Test" OnClick="btnTest_Click" ValidationGroup="General"/>

        </div>
    </div>
    <br />

    <div class="col-sm-12 row"> 

        <div class="col-sm-4 form-inline">
            <asp:Label ID="lblCatalog" runat="server" class="col-sm-4" Text="Catalog" for="ddlCatalog"></asp:Label>
            <asp:DropDownList ID="ddlCatalog" runat="server" class="form-control col-sm-8">
                <asp:ListItem Text="BinaryClassification" Value="BinaryClassification" />
            </asp:DropDownList>
        </div> 

        <div class="col-sm-4 form-inline">
            <asp:Label ID="lblTrainer" runat="server" class="col-sm-4" Text="Trainer" for="ddlTrainer"></asp:Label>
            <asp:DropDownList ID="ddlTrainer" runat="server" class="form-control col-sm-8">
                <asp:ListItem Text=" -- Please Select -- " Value="default" />
                <asp:ListItem Text="AveragedPerceptron" Value="AveragedPerceptron" />
                <asp:ListItem Text="LbfgsLogisticRegression" Value="LbfgsLogisticRegression" />
                <asp:ListItem Text="LdSvm" Value="LdSvm" />
                <asp:ListItem Text="LinearSvm" Value="LinearSvm"/>
                <asp:ListItem Text="SdcaLogisticRegression" Value="SdcaLogisticRegression"/>
                <asp:ListItem Text="SdcaNonCalibrated" Value="SdcaNonCalibrated"/>
                <asp:ListItem Text="SgdCalibrated" Value="SgdCalibrated"/>
                <asp:ListItem Text="SgdNonCalibrated" Value="SgdNonCalibrated"/>
            </asp:DropDownList>
        </div> 

        <div class="col-sm-4 form-inline">
            <asp:Label ID="lblDatabase" runat="server" class="col-sm-4" Text="Database" for="ddlDatabase"></asp:Label>
            <asp:DropDownList ID="ddlDatabase" runat="server" class="form-control col-sm-8">
                <asp:ListItem Text=" -- Please Select -- " Value="default" />
                <asp:ListItem Text="BurpSuite" Value="burp_suite" />
                <asp:ListItem Text="Fuzzy DB" Value="fuzzydb" />
                <asp:ListItem Text="OWASP" Value="OWASP" />
                <asp:ListItem Text="Project Dataset" Value="project_dataset"/>
            </asp:DropDownList>
        </div> 

    </div>

    <br /><br />

    <div class="col-sm-12">

        <asp:TextBox class="form-control" ID="txtResult" runat="server" Rows="10" Columns="80" TextMode="MultiLine"></asp:TextBox>

    </div>

    <div class="col-sm-12">
        
        <br />
        <h3>SQL Commands</h3>
        <br />

        <asp:Panel ID="pnlTest" runat="server" style="margin-top:3em"></asp:Panel>
        <br /><br />

    </div>

    <div class="modal fade" id="mInsert" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog" style="max-width: 80%;">
            <div class="modal-content">
                
                <div class="modal-header">                
                    <h4 class="modal-title" id="H2" runat="server">Edit Dialog</h4>
                </div>

                <div class="modal-body" id="modal-body1" style="min-height: 20%">
                    
                    <div class="col-sm-12" runat="server" id="Div1" style="margin-top: 2em;">
                        <div class="col-sm-12">
                            <div class="col-sm-12">
                                <asp:Label ID="Label3" runat="server" class="col-sm-1" Text="SQL Command" for="txtInsertSQL" ></asp:Label>
                                <asp:TextBox ID="txtInsertSQL" runat="server" class="form-control" Rows="2" TextMode="MultiLine" ValidationGroup="Insert"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqInsertSQL" runat="server" ControlToValidate="txtInsertSQL" ErrorMessage="Please enter your SQL Command!" 
                                    ForeColor="#CC3300" ValidationGroup="Insert"></asp:RequiredFieldValidator>
                            </div>
                            <br />
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblInsertLabel" runat="server" class="col-sm-2" Text="Label" for="ddlInsertLabel"></asp:Label>
                                <asp:DropDownList ID="ddlInsertLabel" runat="server" class="form-control col-sm-3">
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <br />
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnInsert" class="btn btn-warning" runat="server" Text="Insert" OnClick="btnInsert_Click" />              
                    </div>

                </div>
            </div>
        </div>
    </div>


    <div class="modal fade" id="mEdit" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog" style="max-width: 80%;">
            <div class="modal-content">
                
                <div class="modal-header">                
                    <h4 class="modal-title" id="mDetailsLabel" runat="server">Edit Dialog</h4>
                </div>

                <div class="modal-body" id="modal-body2" style="min-height: 20%">
                    
                    <div class="col-sm-12" runat="server" id="dEdit" style="margin-top: 2em;">
                        <div class="col-sm-12">
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblEditID" runat="server" class="col-sm-2" Text="ID" for="txtEditID"></asp:Label>
                                <asp:TextBox ID="txtEditID" runat="server" class="form-control col-sm-2" Enabled="False"></asp:TextBox>
                            </div>
                            <br />
                            <div class="col-sm-12">
                                <asp:Label ID="lblEditSQL" runat="server" class="col-sm-1" Text="SQL Command" for="txtEditSQL" ></asp:Label>
                                <asp:TextBox ID="txtEditSQL" runat="server" class="form-control" Rows="2" TextMode="MultiLine" ValidationGroup="Edit"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEditSQL" ErrorMessage="Please enter your SQL Command!" 
                                    ForeColor="#CC3300" ValidationGroup="Edit"></asp:RequiredFieldValidator>
                            </div>
                            <br />
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblEditLabel" runat="server" class="col-sm-2" Text="Label" for="ddlEditLabel"></asp:Label>
                                <asp:DropDownList ID="ddlEditLabel" runat="server" class="form-control col-sm-3">
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <br />
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnSave" class="btn btn-warning" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="Edit" />              
                    </div>

                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="mDelete" tabindex="-1" role="dialog" aria-labelledby="basicModal" aria-hidden="true">
        <div class="modal-dialog" style="max-width: 80%;">
            <div class="modal-content">
                
                <div class="modal-header">                
                    <h4 class="modal-title" id="H1" runat="server">Delete Dialog</h4>
                </div>

                <div class="modal-body" id="modal-body3" style="min-height: 20%">
                    
                    <div class="col-sm-12" runat="server" id="dDelete" style="margin-top: 2em;">
                        <div class="col-sm-12">
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblDeleteID" runat="server" class="col-sm-2" Text="ID" for="txtDeleteID"></asp:Label>
                                <asp:TextBox ID="txtDeleteID" runat="server" class="form-control col-sm-2" ReadOnly = "true"></asp:TextBox>
                            </div>
                            <br />
                            <div class="col-sm-12">
                                <asp:Label ID="lblDeleteSQL" runat="server" class="col-sm-1" Text="SQL Command" for="txtDeleteSQL" ></asp:Label>
                                <asp:TextBox ID="txtDeleteSQL" runat="server" class="form-control" Rows="2" TextMode="MultiLine" ReadOnly = "true"></asp:TextBox>
                            </div>
                            <br />
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblDeleteLabel" runat="server" class="col-sm-2" Text="Label" for="txtDeleteLabel"></asp:Label>
                                <asp:TextBox ID="txtDeleteLabel" runat="server" class="form-control col-sm-2" ReadOnly = "true"></asp:TextBox>
                            </div>
                            <br />
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <asp:Button ID="btnDelete" class="btn btn-warning" runat="server" Text="Delete" OnClick="btnDelete_Click" />              
                    </div>

                </div>
            </div>
        </div>
    </div>

    
    <script type="text/javascript" src="Test.js"></script> 

</asp:Content>
