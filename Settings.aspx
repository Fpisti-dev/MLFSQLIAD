<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="MLFSQLIAD.Settings" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >

    <h2><%: Title %> Page</h2>

    <asp:HiddenField ID="hTab" runat="server" /> 
    <asp:HiddenField ID="hID" runat="server" />  

    <div class="col-sm-12 row">
        <div class="col-sm-10"></div>
        <div class="col-sm-2">
            <asp:Button ID="btnAdd" runat="server" class="btn btn-primary pull-right" Text="Add New SQL" OnClick="btnAdd_Click" CausesValidation="False" />
        </div>
    </div>
    
    

    <hr />

    
    <ul class="nav nav-tabs" id="myTab" role="tablist">
        <li class="nav-item">
            <a class="nav-link active" id="general-tab" data-toggle="tab" href="#general" role="tab" aria-controls="general" aria-selected="true">General setting</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="burp-tab" data-toggle="tab" href="#burp" role="tab" aria-controls="burp" aria-selected="false">BurpSuite</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="fuzzy-tab" data-toggle="tab" href="#fuzzy" role="tab" aria-controls="fuzzy" aria-selected="false">Fuzzy DB</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="owasp-tab" data-toggle="tab" href="#owasp" role="tab" aria-controls="owasp" aria-selected="false">OWASP</a>
        </li>
        <li class="nav-item">
            <a class="nav-link" id="project-tab" data-toggle="tab" href="#project" role="tab" aria-controls="project" aria-selected="false">Project Dataset</a>
        </li>
    </ul>




    <div class="tab-content" id="myTabContent">
        
        <div class="tab-pane fade show active" id="general" role="tabpanel" aria-labelledby="home-tab">

            <br /><br />

		    <div class="panel-body">
            
                <fieldset class="col-md-12">    	
				    <legend>Please select a Catalog, Trainer and Database</legend>
					
				    <div class="panel panel-default">
				        <div class="panel-body">

                            <div class="col-sm-12 row">
                                
                                <div class="col-sm-4">
                                    <p>Catalogs</p>
                                    <div class="form-check">
                                        <asp:RadioButtonList class="form-check" ID="rblCatalog" runat="server">
                                            <asp:ListItem Text="&nbsp;BinaryClassification" Value="BinaryClassification" />
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <p>Trainers</p>
                                    <div class="form-check">
                                        <asp:RadioButtonList class="form-check" ID="rblTrainer" runat="server">
                                            <asp:ListItem Text="&nbsp;AveragedPerceptron" Value="AveragedPerceptron" />
                                            <asp:ListItem Text="&nbsp;LbfgsLogisticRegression" Value="LbfgsLogisticRegression" />
                                            <asp:ListItem Text="&nbsp;LdSvm" Value="LdSvm" />
                                            <asp:ListItem Text="&nbsp;LinearSvm" Value="LinearSvm"/>
                                            <asp:ListItem Text="&nbsp;SdcaLogisticRegression" Value="SdcaLogisticRegression"/>
                                            <asp:ListItem Text="&nbsp;SdcaNonCalibrated" Value="SdcaNonCalibrated"/>
                                            <asp:ListItem Text="&nbsp;SgdCalibrated" Value="SgdCalibrated"/>
                                            <asp:ListItem Text="&nbsp;SgdNonCalibrated" Value="SgdNonCalibrated"/>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                                <div class="col-sm-4">
                                    <p>Databases</p>
                                    <div class="form-check">
                                        <asp:RadioButtonList class="form-check" ID="rblDatabase" runat="server">
                                            <asp:ListItem Text="&nbsp;BurpSuite" Value="burp_suite" />
                                            <asp:ListItem Text="&nbsp;Fuzzy DB" Value="fuzzydb" />
                                            <asp:ListItem Text="&nbsp;OWASP" Value="OWASP" />
                                            <asp:ListItem Text="&nbsp;Project Dataset" Value="project_dataset"/>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>

                            </div>

                            <div class="col-sm-12 row">
                                <div class="col-sm-11"></div>
                                <div class="col-sm-1">
                                    <asp:Button ID="btnSaveSetup" class="btn btn-warning" runat="server" Text="Save" OnClick="btnSaveSetup_Click" CausesValidation="False"/> 
                                </div>
                            </div>

					    </div>
				    </div>
			    </fieldset>
            </div>
        </div>


        <div class="tab-pane fade" id="burp" role="tabpanel" aria-labelledby="burp-tab">
            <asp:Panel ID="pnlBurp" runat="server" style="margin-top:3em"></asp:Panel>
        </div>
  
        <div class="tab-pane fade" id="fuzzy" role="tabpanel" aria-labelledby="fuzzy-tab">
            <asp:Panel ID="pnlFuzzy" runat="server" style="margin-top:3em"></asp:Panel>
        </div>

        <div class="tab-pane fade" id="owasp" role="tabpanel" aria-labelledby="owasp-tab">
            <asp:Panel ID="pnlOwasp" runat="server" style="margin-top:3em"></asp:Panel>
        </div>

        <div class="tab-pane fade" id="project" role="tabpanel" aria-labelledby="project-tab">
            <asp:Panel ID="pnlProject" runat="server" style="margin-top:3em"></asp:Panel>
        </div>
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
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblInserDatabase" runat="server" class="col-sm-3" Text="Database" for="ddlInserDatabase"></asp:Label>
                                <asp:DropDownList ID="ddlInserDatabase" runat="server" class="form-control col-sm-4">
                                    <asp:ListItem Text="BurpSuite" Value="burp"></asp:ListItem>
                                    <asp:ListItem Text="Fuzzy DB" Value="fuzzy"></asp:ListItem>
                                    <asp:ListItem Text="OWASP" Value="owasp"></asp:ListItem>
                                    <asp:ListItem Text="Project Dataset" Value="project"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <br />
                            <br />
                            <div class="col-sm-12">
                                <asp:Label ID="Label3" runat="server" class="col-sm-1" Text="SQL Command" for="txtInsertSQL" ></asp:Label>
                                <asp:TextBox ID="txtInsertSQL" runat="server" class="form-control" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqInsertSQL" runat="server" ControlToValidate="txtInsertSQL" ErrorMessage="Please enter your SQL Command!" ForeColor="#CC3300"></asp:RequiredFieldValidator>
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
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblEditDB" runat="server" class="col-sm-3" Text="Database" for="txtEditDB"></asp:Label>
                                <asp:TextBox ID="txtEditDB" runat="server" class="form-control col-sm-6" Enabled="False"></asp:TextBox>
                            </div>
                            <br />
                            <div class="col-sm-12">
                                <asp:Label ID="lblEditSQL" runat="server" class="col-sm-1" Text="SQL Command" for="txtEditSQL" ></asp:Label>
                                <asp:TextBox ID="txtEditSQL" runat="server" class="form-control" Rows="2" TextMode="MultiLine"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEditSQL" ErrorMessage="Please enter your SQL Command!" ForeColor="#CC3300" ValidationGroup="Edit"></asp:RequiredFieldValidator>
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
                            <div class="col-sm-6 form-inline">
                                <asp:Label ID="lblDeleteDB" runat="server" class="col-sm-3" Text="Database" for="txtDeleteDB"></asp:Label>
                                <asp:TextBox ID="txtDeleteDB" runat="server" class="form-control col-sm-6" ReadOnly = "true"></asp:TextBox>
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



    <script type="text/javascript" src="Settings.js"></script> 



</asp:Content>

