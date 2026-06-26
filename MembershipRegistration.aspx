<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MembershipRegistration.aspx.vb" Inherits="NewMembership.MembershipRegistration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UAT Membership</title>
       <style>
       body {
    font-family: Arial, sans-serif;
    background-color: #f0f0f0;
    margin: 0;
    padding: 0;
    font-size:12px
}

.container {
    width: 80%;
    max-width: 800px;
    margin: 10px auto;
    background-color: white;
    padding: 15px;
    border-radius: 10px;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
}

h2 {
    text-align: center;
    color: #333;
    margin-bottom: 10px;
    font-size: 1.5em;
}

table {
    width: 100%;
    border-collapse: collapse;
    margin-top: 10px;
}

td {
    padding: 5px;
    vertical-align: top;
    padding-right: 15px;
    width: 50%;
}

.form-group label {
    display: block;
    margin-bottom: 3px;
    color: #333;
    font-size: 0.9em;
}

.form-control {
    width: 100%;
    padding: 8px;
    border: 1px solid #ccc;
    border-radius: 5px;
    box-sizing: border-box;
    font-size: 0.9em;
}

.btn-group {
    text-align: center;
    margin-top: 15px;
}

.btn-group input[type="submit"],
.btn-group input[type="reset"] {
    padding: 8px 15px;
    border: none;
    background-color: #0066cc;
    color: white;
    border-radius: 5px;
    cursor: pointer;
    font-size: 0.9em;
}

      .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.7);
            display: none;
            justify-content: center;
            align-items: center;
            z-index: 1000;
        }

     .overlay-content {
            background-color: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
            max-width: 400px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);
        }

      .overlay-content h3 {
            margin: 0 0 10px;
        }
          .progress-bar-container {
            width: 100%;
            height: 20px;
            background-color: #f3f3f3;
            border-radius: 5px;
            margin: 20px 0;
            overflow: hidden;
        }

        .progress-bar {
            height: 100%;
            background-color: blue;
            width: 0%;
            transition: width 1s linear;
        }

    .overlay-close {
        margin-top: 15px;
        background-color: #cc0000;
        color: white;
        border: none;
        padding: 10px 20px;
        border-radius: 5px;
        cursor: pointer;
    }
        .overlay-close:disabled {
            background-color: #aaaaaa;
            cursor: not-allowed;
        }

.btn-group input[type="reset"] {
    background-color: #cc0000;
}

.text-danger {
    color: red;
    font-size: 0.8em;
}

           .auto-style2 {
               color: #000066;
           }
           .auto-style4 {
               text-align: left;
           }
           .auto-style5 {
               height: 73px;
           }
           .auto-style6 {
               height: 73px;
               color: #0000CC;
           }
           .auto-style7 {
               text-align: left;
               color: #000066;
           }
           </style>
    <script type="text/javascript">
        function validateIDNumber() {
            const idNumber = $('#txtIDNumber').val();
            let errorMsg = '';

            // 1. Length check
            if (idNumber.length !== 13 || !/^\d+$/.test(idNumber)) {
                errorMsg = "ID Number must be exactly 13 digits long and numeric.";
            } else {
                // 2. Birthdate validation
                const birthDate = idNumber.substr(0, 6);
                const year = parseInt(birthDate.substr(0, 2), 10) + (birthDate.substr(0, 2) < '50' ? 2000 : 1900);
                const month = parseInt(birthDate.substr(2, 2), 10) - 1;
                const day = parseInt(birthDate.substr(4, 2), 10);
                const date = new Date(year, month, day);

                if (date.getFullYear() !== year || date.getMonth() !== month || date.getDate() !== day) {
                    errorMsg = "Invalid birth date in ID Number.";
                }

                // 3. Gender validation
                const genderCode = parseInt(idNumber.substr(6, 4), 10);
                if (genderCode < 0 || genderCode > 9999) {
                    errorMsg = "Invalid gender indicator in ID Number.";
                }

                // 4. Citizenship validation
                const citizenship = idNumber[10];
                if (citizenship !== '0' && citizenship !== '1') {
                    errorMsg = "Invalid citizenship indicator in ID Number.";
                }

                // 5. Checksum validation using Luhn algorithm
                if (!isValidLuhn(idNumber)) {
                    errorMsg = "Invalid ID Number checksum.";
                }
            }

            $('#error-msg').text(errorMsg);
            if (!errorMsg) {
                errorMsg = "South African ID Number is Checked and Valid"
            }
          

        }

        // Luhn algorithm for checksum validation
        function isValidLuhn(idNumber) {
            let sum = 0;
            for (let i = 0; i < idNumber.length; i++) {
                let num = parseInt(idNumber[i]);
                if (i % 2 === 0) {
                    sum += num;
                } else {
                    num *= 2;
                    if (num > 9) num -= 9;
                    sum += num;
                }
            }
            return sum % 10 === 0;
        }
    </script>
    <script type="text/javascript">
        function showOverlay() {
            const overlay = document.getElementById("overlay");
            const progressBar = document.getElementById("progressBar");
            const closeButton = document.getElementById("closeButton");

            // Show the overlay
            overlay.style.display = "flex";

            // Reset progress bar and disable the Close button
            progressBar.style.width = "0%";
            closeButton.disabled = true;

            let progress = 0;

            // Simulate progress bar filling over 60 seconds
            const interval = setInterval(() => {
                progress += 1; // Increment progress by 1%
                progressBar.style.width = progress + "%";

                if (progress >= 100) {
                    clearInterval(interval); // Stop interval when progress reaches 100%
                    closeButton.disabled = false; // Enable the Close button
                }
            }, 300); // 600ms per 1% for a total of 60 seconds
        }

        function closeOverlay() {
            document.getElementById("overlay").style.display = "none";
        }

        let selectedAmount = 0;

        // Set the amount based on subscription choice
        function setAmount(amount) {
            selectedAmount = amount;
            document.getElementById('payAmount').value = selectedAmount.toFixed(2); // Update Payfast amount
        }

        function showPaymentFields(option) {
            // Hide all payment fields initially
            document.getElementById("paymentGateway").style.display = "none";
            document.getElementById("uploadProofOfPayment").style.display = "none";

            // Show the relevant fields based on the selected option
            if (option === 'EFT') {
                document.getElementById("uploadProofOfPayment").style.display = "block";
            } else if (option === 'Online') {
                document.getElementById("paymentGateway").style.display = "block";
            } else if (option === 'Free') {
                alert("Please enter your authorisation code for free membership.");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
  <div id="overlay" class="overlay">
        <div class="overlay-content">

            <h3>Processing your registration...</h3>
            <p>Please wait while we validate your information.</p>
            <div class="progress-bar-container">
                <div id="progressBar" class="progress-bar"></div>
            </div>
            <button id="closeButton" class="overlay-close" onclick="closeOverlay()" disabled="disabled">Close</button>
        </div>
    </div>
       <div class="container">
         
            <h2 color="blue" class="auto-style2">UAT MEMBERSHIP FORM</h2>
            <h5><asp:Label ID="lblMessage" runat="server" CssClass="auto-style6" ForeColor="#0000CC"></asp:Label></h5>
           
            <table>
                
                <tr>
                    <td class="auto-style4">
                             <label for="idNumber">ID Number:</label><br /><asp:TextBox ID="txtIDNumber" runat="server" TextMode="Number" CssClass="form-control" oninput="validateIDNumber()" AutoPostBack="True"/>
     <span id="error-msg" style="color: red;"></span>
     <asp:RequiredFieldValidator ID="rfvIDNumber" runat="server" ControlToValidate="txtIDNumber"
         ErrorMessage="ID Number is required." Display="Dynamic" CssClass="text-danger" />
     <asp:RegularExpressionValidator ID="revIDNumber" runat="server" ControlToValidate="txtIDNumber"
         ValidationExpression="^\d{13}$" ErrorMessage="Enter a valid 13-digit South African ID number." Display="Dynamic" CssClass="text-danger" />
  <asp:TextBox ID="txtAge" runat="server" TextMode="Number" CssClass="form-control" Visible="false"/>
                        
                    </td>
            
                    <td>

             <label for="ddllanguage">Prefered Language:</label><br /><asp:DropDownList ID="ddllanguage" runat="server" CssClass="form-control" AutoPostBack="True">
                           <asp:ListItem Value="0">Select language</asp:ListItem>
  <asp:ListItem Value="1">Afrikaans</asp:ListItem>
   <asp:ListItem Value="2">English</asp:ListItem>
   <asp:ListItem Value="3">IsiNdebele</asp:ListItem>
   <asp:ListItem Value="4">IsiXhosa</asp:ListItem>
   <asp:ListItem Value="5">IsiZulu</asp:ListItem>
   <asp:ListItem Value="6">Sepedi (Northern Sotho)</asp:ListItem>
   <asp:ListItem Value="7">Sesotho</asp:ListItem>
   <asp:ListItem Value="8">Setswana</asp:ListItem>
   <asp:ListItem Value="9">SiSwati</asp:ListItem>
   <asp:ListItem Value="10">Tshivenda</asp:ListItem>
   <asp:ListItem Value="11">Xitsonga</asp:ListItem>
   <asp:ListItem Value="12">Sign Language</asp:ListItem>
   <asp:ListItem Value="13">Shona</asp:ListItem>
   <asp:ListItem Value="14">Portuguese</asp:ListItem>
   <asp:ListItem Value="15">Ndau</asp:ListItem>
                       </asp:DropDownList>
                       <asp:RequiredFieldValidator ID="rfvLanguage" runat="server" ControlToValidate="ddllanguage"
                           ErrorMessage="Language is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                </tr>
                <!-- ID Number Field -->
                <tr>
                    <td class="auto-style4">
                                           <label for="txtFirstName">Name:</label><br /><asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtFirstName"
                            ErrorMessage="Name is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
            
                    <td>
 
                        <label for="txtLastName">Surname:</label><br /><asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvtxtLastName" runat="server" ControlToValidate="txtLastName"
                            ErrorMessage="Surname is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                </tr>
                <!-- Gender Field -->
                <tr>
                    <td class="auto-style4">
                        <label for="gender">Gender:</label><br /><asp:DropDownList ID="ddlgender" runat="server" CssClass="form-control">
                            <asp:ListItem>Select Gender</asp:ListItem>
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlgender"
                            InitialValue="Select Gender" ErrorMessage="Gender is required." Display="Dynamic" CssClass="text-danger" />
                   
                        </td>
               
                    <td>
                        <label for="subscription">Subscription Type:</label><br /><asp:DropDownList ID="ddlSubscriptionType" runat="server" CssClass="form-control">
                            <asp:ListItem>Select Subscription</asp:ListItem>
                          <%--<asp:ListItem>Donation</asp:ListItem>--%>
                            <asp:ListItem>Renewal</asp:ListItem>
                            <asp:ListItem>New Registration</asp:ListItem>
                        </asp:DropDownList>
              <asp:RequiredFieldValidator ID="RFVSubscribe" runat="server" ControlToValidate="ddlSubscriptionType"
                            InitialValue="Select Subscription Type:" ErrorMessage="Subscription Type is required." Display="Dynamic" CssClass="text-danger" />
		</td></tr>
                 <tr>
                    <td>
    
                        Are you a
                        <label for="registeredVoter">registered voter?</label><br /><asp:DropDownList ID="ddlregisteredVoter" runat="server" CssClass="form-control">
        <asp:ListItem>Please Select</asp:ListItem>
        <asp:ListItem>No</asp:ListItem>
     <asp:ListItem>Yes</asp:ListItem>
               </asp:DropDownList>
    <asp:RequiredFieldValidator ID="RFVVoter" runat="server" ControlToValidate="ddlregisteredVoter"
    ErrorMessage="Response required." Display="Dynamic" CssClass="text-danger" />
                     </td>
         <td>             
             <label for="ddlCountry">Country of Residence:</label><br /><asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control">

    <asp:ListItem>South Africa</asp:ListItem>
     <asp:ListItem>Lesotho</asp:ListItem>
    <asp:ListItem>Botswana</asp:ListItem>
    <asp:ListItem>Mozambique</asp:ListItem>
    <asp:ListItem>Zimbabwe</asp:ListItem>
  <asp:ListItem>Eswatini</asp:ListItem>
    <asp:ListItem>Tanzania</asp:ListItem>
    <asp:ListItem>United Kingdom</asp:ListItem>
             <asp:ListItem>United States of America</asp:ListItem>
             <asp:ListItem>China</asp:ListItem>
              <asp:ListItem>Saudi Arabia</asp:ListItem>
                      <asp:ListItem>Australia</asp:ListItem>
              <asp:ListItem>Italy</asp:ListItem>
               <asp:ListItem>Japan</asp:ListItem>
         <asp:ListItem>Other..</asp:ListItem>
        </asp:DropDownList>
 <asp:RequiredFieldValidator ID="RFVCountry" runat="server" ControlToValidate="ddlCountry"
 ErrorMessage="Country is required." Display="Dynamic" CssClass="text-danger" />
         </td></tr>
                <tr><td>
             <label for="email">Email Address:</label><br /><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="email" />
            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address." Display="Dynamic" CssClass="text-danger" /> </td>
                     <td> 
      <label for="cell">Cell No:</label><br /><asp:TextBox ID="txtCellNumber" runat="server" CssClass="form-control" placeholder="0123456789" CausesValidation="True" TextMode="Number" />
<asp:RequiredFieldValidator ID="rfvCell" runat="server" ControlToValidate="txtCellNumber" ErrorMessage="Cell No is required." Display="Dynamic" CssClass="text-danger" />
&nbsp;<asp:RegularExpressionValidator ID="revCell" runat="server" ControlToValidate="txtCellNumber" ValidationExpression="^\d{10}$" ErrorMessage="Enter a valid 10-digit cell number." Display="Dynamic" CssClass="text-danger" />  </td></tr>
 
                <tr>
                    <td class="auto-style4">
                        <label for="ddlProvince">Province:</label><br /><asp:DropDownList ID="ddlProvince" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince"
                            InitialValue="" ErrorMessage="Province is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
              
                    <td>
                        
                        <label for="residentialAddress">Residential Address:</label><br /><asp:TextBox ID="txtResidentialAddress" runat="server" CssClass="form-control" Height="43px" TextMode="MultiLine" /><br />
                        <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtResidentialAddress"
                            ErrorMessage="Residential Address is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                </tr>
                <!-- Local Municipality Field -->
                <tr>
                    <td class="auto-style17">
                        
                        <label for="ddlDistrictMunicipality">District Municipality/ Metropolitan:</label><br /><asp:DropDownList ID="ddlDistrictMunicipality" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlDistrictMunicipality_SelectedIndexChanged"/>
                        <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="ddlDistrictMunicipality" ErrorMessage="District Municipality is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
              
                    <td class="auto-style16">
                        <label for="postalCode">Postal Code:</label><br /><asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" TextMode="Number" />
      <asp:RegularExpressionValidator ID="revPostalCode" runat="server" ControlToValidate="txtPostalCode" ValidationExpression="^\d{4}$" ErrorMessage="Enter a valid 4-digit Postal Code." Display="Dynamic" CssClass="text-danger" />
                    </td>
                </tr>
                <!-- Residential Address Field -->
                <tr>
                    <td class="auto-style4">
                        
                        <label for="ddlLocalMunicipality" width="300px">Local Municipality:</label><br /><asp:DropDownList ID="ddlLocalMunicipality" runat="server" CssClass="form-control" AutoPostBack="True" />
                        <asp:RequiredFieldValidator ID="rfvLocal" runat="server" ControlToValidate="ddlLocalMunicipality" ErrorMessage="Local Municipality is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
              
                    <td>
                        
                        <label for="ddlLocation" width="300px">Location/ City</label><br /><asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="ddlLocation" ErrorMessage="Location is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        
                        <label for="WardNo">Ward No:</label><br /><asp:DropDownList ID="ddlWardNo" runat="server" CssClass="form-control" />
                         <asp:RequiredFieldValidator ID="RFVWardNo" runat="server" ControlToValidate="ddlWardNo" ErrorMessage="Ward Number is required." Display="Dynamic" CssClass="text-danger" />
                    </td>
              
                    <td>
                        Tick if Disabled:<br /><strong><asp:CheckBox ID="ckDisability" Text="Person Living with Disability" runat="server" CssClass="auto-style12"/>
                        </strong>
                    </td>
                </tr>
                 <tr><td class="auto-style5">
                        <label for="postalCode">University/ College /Tertiary Students:</label><br /><asp:DropDownList ID="ddlUniversity" runat="server" CssClass="form-control" />
                     <br /><asp:TextBox ID="txtTelephoneH" runat="server" CssClass="form-control" placeholder="0123456789" Visible="False"   />
      <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtTelephoneH" ValidationExpression="^\d{10}$" ErrorMessage="Enter a valid 10-digit Telephone number." Display="Dynamic" CssClass="text-danger" />
   </td>
     <td class="auto-style5"> <label for="telephoneW">Telephone(W):</label><br /><asp:TextBox ID="txtTelephoneW" runat="server" CssClass="form-control" placeholder="0123456789"  />
   <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtTelephoneW" ValidationExpression="^\d{10}$" ErrorMessage="Enter a valid 10-digit Telephone number." Display="Dynamic" CssClass="text-danger" />
                                                           </td></tr>
      <tr>
        <td>
            <strong style="font-size:12px">Tick Membership Subscription:</strong><br />
            <asp:RadioButton ID="annualFee" runat="server" GroupName="MembershipFee" Text="R20 - Annual Fee" style="font-size: 11px;" OnClientClick="setAmount(20)" />
            <br />
            <asp:RadioButton ID="fiveYearFee" runat="server" GroupName="MembershipFee" Text="R100 - (5) Years Fee" style="font-size: 11px;" OnClientClick="setAmount(100)" />
        </td>
        <td>
            <strong>Select the type of payment for your subscription</strong><br />
            <asp:RadioButton ID="EFT" runat="server" GroupName="PaymentType" Text="EFT/Bank Deposit - I will upload the Proof of Payment" style="font-size: 11px;" onclick="showPaymentFields('EFT')" />
            <br />
            <asp:RadioButton ID="Online" runat="server" GroupName="PaymentType" Text="Online Payment - I will make immediate payment online" style="font-size: 11px;" onclick="showPaymentFields('Online')" />
            <br />
            <%--<asp:RadioButton ID="Free" runat="server" GroupName="PaymentType" Text="Free Membership - Authorisation Code required" style="font-size: 11px;" onclick="showPaymentFields('Free')" />--%>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
     <tr>
            <td class="auto-style2">&nbsp;</td>
            <td>
                <div id="paymentGateway" style="display: none;">
                    <strong>Online Payment Gateway:</strong> <br />
                    <!-- Required hidden fields for Payfast -->
                    <form action="https://www.payfast.co.za/eng/process" method="post" id="payfastForm">
                        <input type="hidden" name="merchant_id" value="17349143" /> <!-- Replace with your Merchant ID -->
                        <input type="hidden" name="merchant_key" value="szcy1nvrham98" /> <!-- Replace with your Merchant Key -->
                        <input type="hidden" name="return_url" value="https://www.uatmembership.org.za/MembershipRegistration.aspx" />
                        <input type="hidden" name="cancel_url" value="https://www.uatmembership.org.za/cancel.aspx" />
                        <input type="hidden" name="notify_url" value="https://www.uatmembership.org.za/MembershipRegistration.aspx" />
                        <input type="hidden" name="amount" id="payAmount" value="0.00" />
                        <input type="hidden" name="item_name" value="Membership Subscription" />
                        <!-- Pay Now Button -->
                        <button type="submit" style="background-color: #4CAF50; color: white; font-size: 12px; padding: 10px; border: none; cursor: pointer; width:80%">
                            Pay Now
                        </button>
                    </form>
                </div>
                <br />
                <div id="uploadProofOfPayment" style="display: none;">
                    <strong>Upload Proof of Payment:</strong>
                    <asp:FileUpload ID="UploadPoP" CssClass="form-control" runat="server" />
                    <br />
                    <span>BANK: <strong>Standard Bank;</strong> Account Name: <strong>UAT;</strong> <br />
                    Account No: <strong>000615617;</strong> Reference: <strong>Membership</strong></span>
                </div>
            </td>
        </tr>
            </table>
               
        <div class="declaration">
			  <h3 class="auto-style7">DECLARATION:</h3>
            <p style="font-size: smaller">I 
                <span class="auto-style2">Doer</span><strong>: 
                <asp:TextBox ID="DeclareName" runat="server" Width="200px" CssClass="form-control" Placeholder="Name and Surname"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="revdeclare" runat="server" ControlToValidate="DeclareName"
                            ErrorMessage="Declaration is required." Display="Dynamic" CssClass="text-danger" />
                </strong>&nbsp;<span>solemnly declare my unwavering commitment to the Vision and
Objectives of the <span class="auto-style15"> <strong>United Africans Transformation (UAT)</strong></span>, as enshrined in the Party’s Constitution and Policy Document. I
am joining UAT on my own free will, without coercion, and with no motive formaterial gain or personal advantage.
I pledge to respect the Executive Leadership and the Structures of the Party, and to serve as a dedicated and loyal member. I
will contribute my energies and skills to the service of the organization, faithfully executing any tasks assigned to me. My
commitment is to strengthen UAT as an effective movement for the liberation and advancement of our society.
I vow to uphold and defend the unity and integrity of the organization and its Principles, and to actively work against any
disruptions or factionalism that may threaten our movement. </span></p>
        </div>

              <div class="PaymentAdmin" id="PaymentAdmin" runat="server">
                  <br />  <h3 class="auto-style7">Payment Confirmation:</h3>
                               <strong style="font-size:12px">Tick Below, On the paid amount as per the Proof of Payment Received :</strong><br />
                <asp:RadioButton ID="RadioButton1" runat="server" GroupName="PaidFee" Text="R20 PAID - 1 Year Annual Fee" style="font-size: 11px;" /> 
                  <br /><asp:RadioButton ID="RadioButton2" runat="server" GroupName="PaidFee" Text="R100 PAID- 5 Years Annual Fee" style="font-size: 11px;" />
         <br /> <br /><strong><asp:Button ID="btnUpdate" runat="server" Text="Confirm Payment" CssClass="btn btn-primary" BackColor="BlueViolet" ForeColor="White" style="font-weight: bold; background-color: #FF0000" Visible="False"/>
                  </strong>&nbsp;
                  </div>
        <div class="btn-group">
            <asp:Button ID="submitButton" runat="server" Text="Submit" CssClass="btn btn-primary" OnClientClick ="showOverlay()" />
            &nbsp;
            <asp:Button ID="resetButton" runat="server" Text="Reset" CssClass="btn btn-secondary" CausesValidation="False" /><asp:TextBox ID="txtSubscriptionDate" Width="50px" runat="server" TextMode="Date" CssClass="form-control" Visible="False" /> <asp:TextBox ID="txtMemberNo" Width="50px" runat="server" Visible="false"></asp:TextBox>
        <asp:HiddenField ID="hdnMemberNo" runat="server"/>
            <asp:HiddenField ID="hdnMemberID" runat="server" />
        </div>
   </div>
    </form>
</body>
</html>
