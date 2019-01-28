@echo off

REM
REM -- Make all the directories
REM
mkdir wwwroot\lib\chartjs
mkdir wwwroot\lib\font-awesome 
mkdir wwwroot\lib\font-awesome\css
mkdir wwwroot\lib\font-awesome\fonts
mkdir wwwroot\lib\jquery
mkdir wwwroot\lib\jquery-ui
mkdir wwwroot\lib\jquery-validation
mkdir wwwroot\lib\jquery-validation-unobtrusive
mkdir wwwroot\lib\moment
mkdir wwwroot\lib\bootstrap
mkdir wwwroot\lib\bootstrap\css
mkdir wwwroot\lib\bootstrap\js
mkdir wwwroot\lib\morris
mkdir wwwroot\lib\datatables
mkdir wwwroot\lib\datatables\autofill
mkdir wwwroot\lib\datatables\buttons
mkdir wwwroot\lib\datatables\colreorder
mkdir wwwroot\lib\datatables\datatables
mkdir wwwroot\lib\datatables\fixedcolumns
mkdir wwwroot\lib\datatables\fixedheader
mkdir wwwroot\lib\datatables\jszip
mkdir wwwroot\lib\datatables\keytable
mkdir wwwroot\lib\datatables\pdfmake
mkdir wwwroot\lib\datatables\responsive
mkdir wwwroot\lib\datatables\rowgroup
mkdir wwwroot\lib\datatables\rowreorder
mkdir wwwroot\lib\datatables\scroller
mkdir wwwroot\lib\datatables\select

REM
REM -- Datatables
REM

echo Datatables...
copy node_modules\datatables.net\js\jquery.dataTables.js wwwroot\lib\datatables
copy node_modules\datatables.net-bs4\js\dataTables.bootstrap4.js wwwroot\lib\datatables
copy node_modules\datatables.net-bs4\css\dataTables.bootstrap4.css wwwroot\lib\datatables

echo Datatables-AutoFill...
copy node_modules\datatables.net-autofill\js\dataTables.autoFill.min.js wwwroot\lib\datatables\autofill
copy node_modules\datatables.net-autofill-bs4\js\autoFill.bootstrap4.min.js wwwroot\lib\datatables\autofill
copy node_modules\datatables.net-autofill-bs4\css\autoFill.bootstrap4.min.css wwwroot\lib\datatables\autofill

echo Datatables-Buttons...
copy node_modules\datatables.net-buttons\js\buttons.colVis.min.js wwwroot\lib\datatables\buttons
copy node_modules\datatables.net-buttons\js\buttons.flash.min.js wwwroot\lib\datatables\buttons
copy node_modules\datatables.net-buttons\js\buttons.html5.min.js wwwroot\lib\datatables\buttons
copy node_modules\datatables.net-buttons\js\buttons.print.min.js wwwroot\lib\datatables\buttons
copy node_modules\datatables.net-buttons\js\datatables.buttons.min.js wwwroot\lib\datatables\buttons
copy node_modules\datatables.net-buttons-bs4\js\buttons.bootstrap4.min.js wwwroot\lib\datatables\buttons
copy node_modules\datatables.net-buttons-bs4\css\buttons.bootstrap4.min.css wwwroot\lib\datatables\buttons

echo Datatables-ColReorder...
copy node_modules\datatables.net-colReorder\js\dataTables.colReorder.min.js wwwroot\lib\datatables\colReorder
copy node_modules\datatables.net-colReorder-bs4\css\colReorder.bootstrap4.min.css wwwroot\lib\datatables\colReorder

echo Datatables-FixedColumns...
copy node_modules\datatables.net-fixedColumns\js\dataTables.fixedColumns.min.js wwwroot\lib\datatables\fixedColumns
copy node_modules\datatables.net-fixedColumns-bs4\css\fixedColumns.bootstrap4.min.css wwwroot\lib\datatables\fixedColumns

echo Datatables-FixedHeader...
copy node_modules\datatables.net-fixedHeader\js\dataTables.fixedHeader.min.js wwwroot\lib\datatables\fixedHeader
copy node_modules\datatables.net-fixedHeader-bs4\css\fixedHeader.bootstrap4.min.css wwwroot\lib\datatables\fixedHeader

echo Datatables-JSZip...
copy node_modules\jszip\dist\jszip.min.js wwwroot\lib\datatables\jszip

echo Datatables-KeyTable...
copy node_modules\datatables.net-keyTable\js\dataTables.keyTable.min.js wwwroot\lib\datatables\keyTable
copy node_modules\datatables.net-keyTable-bs4\css\keyTable.bootstrap4.min.css wwwroot\lib\datatables\keyTable

echo Datatables-PDFMake...
copy node_modules\pdfmake\build\pdfmake.min.js wwwroot\lib\datatables\pdfmake
copy node_modules\pdfmake\build\vfs_fonts.js wwwroot\lib\datatables\pdfmake

echo Datatables-Responsive...
copy node_modules\datatables.net-responsive\js\datatables.responsive.min.js wwwroot\lib\datatables\responsive
copy node_modules\datatables.net-responsive-bs4\js\responsive.bootstrap4.min.js wwwroot\lib\datatables\responsive
copy node_modules\datatables.net-responsive-bs4\css\responsive.bootstrap4.min.css wwwroot\lib\datatables\responsive

echo Datatables-RowGroup...
copy node_modules\datatables.net-rowGroup\js\dataTables.rowGroup.min.js wwwroot\lib\datatables\rowGroup
copy node_modules\datatables.net-rowGroup-bs4\css\rowGroup.bootstrap4.min.css wwwroot\lib\datatables\rowGroup

echo Datatables-ReOrder...
copy node_modules\datatables.net-rowReorder\js\dataTables.rowReorder.min.js wwwroot\lib\datatables\rowReorder
copy node_modules\datatables.net-rowReorder-bs4\css\rowReorder.bootstrap4.min.css wwwroot\lib\datatables\rowReorder


echo Datatables-Scroller...
copy node_modules\datatables.net-scroller\js\dataTables.scroller.min.js wwwroot\lib\datatables\scroller
copy node_modules\datatables.net-scroller-bs4\css\scroller.bootstrap4.min.css wwwroot\lib\datatables\scroller

echo Datatables-Select...
copy node_modules\datatables.net-select\js\dataTables.select.min.js wwwroot\lib\datatables\select
copy node_modules\datatables.net-select-bs4\css\select.bootstrap4.min.css wwwroot\lib\datatables\select

REM
REM -- Bootstrap
REM
echo Bootstrap...
copy node_modules\bootstrap\dist\css\*.min.css wwwroot\lib\bootstrap\css
copy node_modules\bootstrap\dist\js\*.min.js wwwroot\lib\bootstrap\js
copy node_modules\popper.js\dist\umd\popper.min.js wwwroot\lib\bootstrap\js
copy node_modules\popper.js\dist\umd\popper-utils.min.js wwwroot\lib\bootstrap\js

REM
REM -- Chart JS
REM
echo ChartJS...
copy node_modules\chartjs\chart.js wwwroot\lib\chartjs

REM
REM -- Morris
REM
echo Morris...
copy node_modules\morris.js\morris.min.js wwwroot\lib\morris
copy node_modules\raphael\raphael.min.js wwwroot\lib\morris

REM
REM -- Font Awesome
REM
echo Font Awesome...
copy node_modules\font-awesome\css\font-awesome.min.css wwwroot\lib\font-awesome\css
copy node_modules\font-awesome\fonts\*.* wwwroot\lib\font-awesome\fonts

REM
REM -- Jquery
REM
echo JQuery...
copy node_modules\jquery\dist\jquery.min.js wwwroot\lib\jquery\jquery.js

REM
REM -- Jquery UI
REM
echo JQuery UI...
copy node_modules\jquery-ui-dist\jquery-ui.min.js wwwroot\lib\jquery-ui\jquery-ui.js

REM
REM -- Jquery Validation
REM
echo JQuery Validation...
copy node_modules\jquery-validation\dist\jquery.validate.min.js wwwroot\lib\jquery-validation\jquery.validate.js
copy node_modules\jquery-validation\dist\additional-methods.js wwwroot\lib\jquery-validation
copy node_modules\jquery-validation-unobtrusive\dist\jquery.validate.unobtrusive.js wwwroot\lib\jquery-validation

REM
REM -- Moment
REM
echo Moment...
copy node_modules\moment\min\moment-with-locales.min.js wwwroot\lib\moment\moment.js


REM
REM -- List outdated packages
REM
npm outdated
