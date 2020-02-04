<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentResponse.aspx.cs" Inherits="EEG_ReelCinemasRESTAPI.PaymentResponse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <style>
        /* Center the loader */
        #divMsg {
            font-size: 22px;
            position: absolute;
            left: 50%;
            top: 30%;
            width: 150px;
            height: 150px;
            margin: -75px 0 0 -75px;
        }

        #loader {
            position: absolute;
            left: 50%;
            top: 50%;
            z-index: 1;
            width: 150px;
            height: 150px;
            margin: -75px 0 0 -75px;
            border: 16px solid #f3f3f3;
            border-radius: 50%;
            border-top: 16px solid #3498db;
            width: 120px;
            height: 120px;
            -webkit-animation: spin 2s linear infinite;
            animation: spin 2s linear infinite;
        }

        @-webkit-keyframes spin {
            0% {
                -webkit-transform: rotate(0deg);
            }

            100% {
                -webkit-transform: rotate(360deg);
            }
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }

        /* Add animation to "page content" */
        .animate-bottom {
            position: relative;
            -webkit-animation-name: animatebottom;
            -webkit-animation-duration: 1s;
            animation-name: animatebottom;
            animation-duration: 1s
        }

        @-webkit-keyframes animatebottom {
            from {
                bottom: -100px;
                opacity: 0
            }

            to {
                bottom: 0px;
                opacity: 1
            }
        }

        @keyframes animatebottom {
            from {
                bottom: -100px;
                opacity: 0
            }

            to {
                bottom: 0;
                opacity: 1
            }
        }

        #myDiv {
            display: none;
            text-align: center;
        }

        .btn {
            display: none;
        }
    </style>
</head>
<body onload="myFunction()" style="margin: 0;">
    <form id="form1" runat="server">
        <div>

            <div id="loader"></div>
            <div id="divMsg">
                Your payment is in progress, please do not close or refresh the page
            </div>
            <asp:Button Text="" CssClass="btn" ID="btnAction" OnClick="btnAction_Click" ClientIDMode="Static" runat="server" />
            <script type="text/javascript">
                var myVar;

                function myFunction() {
                    myVar = setTimeout(showPage, 3000);
                }

                function showPage() {
                    document.getElementById("btnAction").click();
                }
            </script>
        </div>
    </form>
</body>
</html>
