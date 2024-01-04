/*Show the whole employee list*/
$(document).ready(function () {
    function refreshEmployeeList() {
        $.ajax({
            url: '/Employee/GetEmployeeList',
            type: 'GET',
            success: function (data) {
                $('#employeeTableBody').html(data);
            },
            error: function () {
                alert('Data cannot be shown!');
            }
        });
    }

    //View Employee
    $(document).on('click', '.view-employee', function () {
        let employeeID = $(this).data('employee-id');

        $.ajax({
            url: '/Employee/GetEmployeeDetails',
            type: 'GET',
            data: { id: employeeID },
            success: function (data) {
                $('#employeeDetails').html(data);
                $('#viewEmployeeModal').modal('show');
            },
            error: function () {
                let errorMessage = 'An error occurred while retrieving employee details.';
                $('#employeeDetails').html('<p>' + errorMessage + '</p>');
                $('#viewEmployeeModal').modal('show');
            }
        });
    });

    //View Edit Employee Modal
    $(document).on('click', '.edit-employee', function () {
        let employeeID = $(this).data('employee-id');
        $.ajax({
            url: '/Employee/GetEmployeeEdit',
            type: 'GET',
            data: { id: employeeID },
            success: function (data) {
                $('#viewEmployeeModal').modal('hide');
                $('#editEmployee').html(data);
                $('#editEmployeeModal').modal('show');
                getPositionOnOpen();
                getGenderOptions("genderUpdate");
                getGenderOnChange("genderUpdate");
                getPositionbyDepartment("updateDpPs");
                imageUpdateChange("imageFileUpdate", "avatarImageUpdate");
            },
            error: function () {
                let errorMessage = 'An error occurred while retrieving employee details.';
                $('#employeeDetails').html('<p>' + errorMessage + '</p>');
                $('#editEmployeeModal').modal('show');
            }
        });
    });

    //Update Employee
    //function imageUpdateChange () {
    //    document.getElementById("imageFileUpdate").onchange = function () {
    //        let reader = new FileReader();
    //        reader.onload = function (e) {
    //            document.getElementById("avatarImageUpdate").src = e.target.result; // get loaded data and render thumbnail.
    //        };
    //        reader.readAsDataURL(this.files[0]); // read the image file as a data URL.
    //    };
    //}
    function imageUpdateChange (fileId, avaId) {
        document.getElementById(fileId).onchange = function () {
            let reader = new FileReader();
            reader.onload = function (e) {
                document.getElementById(avaId).src = e.target.result; // get loaded data and render thumbnail.
            };
            reader.readAsDataURL(this.files[0]); // read the image file as a data URL.
        };
    }

    $('#editEmployeeForm').on('submit', function (e) {
        e.preventDefault();

        let formData = new FormData(this);
        let positionID = $('#updateDpPs [name=positionName]').val();
        let genderName = $("#genderValue").attr('value');
        console.log(genderName + "gender");
        formData.append('positionID', positionID); // Append position ID
        formData.append('gender', genderName);

        $.ajax({
            url: '/Employee/Update',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                $('#updateSuccess').modal('show');
                refreshEmployeeList();
            },
            error: function () {
                alert('Error');
            }
        });
    });

    let employeeID;
    let button;
    //Delete employee
    $(document).off('click', '.delete-employee').on('click', '.delete-employee', function () {
        employeeID = $(this).data('employee-id');
        let firstName = $(this).closest('tr').find('.employee-firstname').text() || $('.employee-firstname-view').text();
        let lastName = $(this).closest('tr').find('.employee-lastname').text() || $('.employee-lastname-view').text();
        button = $(this);
        $('#deleteEmployeeModal').modal('show');
        $(document).off('click', '#deleteConfirm').on('click', '#deleteConfirm', function () {
            $.ajax({
                url: '/Employee/Delete',
                type: 'GET',
                data: { id: employeeID },
                success: function () {
                    button.closest('tr').remove();
                    $('#deleteEmployeeModal').modal('hide');
                    $('#deleteSuccess').modal('show');
                    $('#deleteSuccess .modal-body h4').text('Deleted Successfully');
                    $('#deleteSuccess .modal-body p').text(firstName + ' ' + lastName + ' (' + employeeID + ')');
                    $('#viewEmployeeModal').modal('hide');
                    $('#editEmployeeModal').modal('hide');
                    refreshEmployeeList();
                },
                error: function () {
                    alert('Error deleting');
                }
            });
        });
    });


    //Create Employee
    imageUpdateChange("imageFile", "avatarImage");
    $(function () {
        // Show the modal when the button is clicked
        $('#createEmployeeModalBtn').click(function () {
            $('#createEmployeeModal').modal('show');
        });
    });

    $('#createEmployeeModal').on('show.bs.modal', function () {
        
        getGenderOptions("genderCreate");
    });
    getPositionbyDepartment("createDpPs");

    $('#createEmployeeForm').off('submit').on('submit', function (e) {
        e.preventDefault();

        let formData = new FormData(this);
        formData.append('imageFile', $('#imageFile')[0].files[0]); // Append imageFile element

        let positionID = $('#createDpPs [name=positionName]').val();
        let genderName = $('#createDpPs [name=genderCreate]').val();
        formData.append('positionID', positionID); // Append position ID
        formData.append('gender', genderName);

        if (document.querySelector('.id-right').hidden) {
            $('#createErrorModal').modal('show');
            return false;
        }

        $.ajax({
            url: '/Employee/Create',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                let firstName = $('#firstName').val();
                let lastName = $('#lastName').val();
                let id = $('#ID').val();
                
                $('#createSuccess').modal('show');
                $('#createSuccess .modal-body h4').text(firstName + ' ' + lastName + ' (' + id + ')');
                $('#createSuccess .modal-body p').text('has been created successfully!');
                document.querySelector('.id-right').hidden = true;
                document.querySelector('.id-wrong').hidden = false;
                refreshEmployeeList();
            },
            error: function () {
                alert('Error creating employee');
            }
        });
    });

    $(document).on('click', '#createEmployeeForm .btn-clear', function () {
        document.getElementById('createEmployeeForm').reset();
        document.getElementById('avatarImage').src = "/images/ava-default.png";
        document.getElementById('imageFile').value = "";
        document.getElementById("createDpPs").querySelector("#positionLabel").hidden = true;
        document.getElementById("createDpPs").querySelector("#positionName").hidden = true;
        document.querySelector('.id-right').hidden = true;
        document.querySelector('.id-wrong').hidden = true;

    });
    $('#ID').keyup(function () {
        iDCheck();
    });

    function iDCheck() {
        const currentId = $('#createEmployeeForm [name=ID]').val();
        $.ajax({
            url: '/Employee/GetEmployeeDetails',
            type: 'GET',
            data: { id: currentId },
            success: function (response) {
                if (response.error) {
                    document.querySelector('.id-right').hidden = false;
                    document.querySelector('.id-wrong').hidden = true;
                }
                else {
                    document.querySelector('.id-right').hidden = true;
                    document.querySelector('.id-wrong').hidden = false;
                }
            },
            error: function () {
                document.querySelector('.id-right').hidden = true;
                document.querySelector('.id-wrong').hidden = true;
            }
        });
    }

    //Get Positions by Department
    function getPositionOnOpen () {
        let departmentID = $('#updateDpPs [name=departmentName]').val();
        let positionDropdown = document.getElementById("updateDpPs").querySelector("#positionName");
        positionDropdown.innerHTML = "";

        if (departmentID !== "") {
            $.ajax({
                url: '/Employee/GetPositionsByDepartment?departmentID=' + departmentID,
                type: 'GET',
                success: function (data) {
                    document.getElementById("updateDpPs").querySelector("#positionLabel").hidden = false;
                    document.getElementById("updateDpPs").querySelector("#positionName").hidden = false;

                    let positionValue = $("#positionValue").attr('value');

                    let defaultOption = document.createElement("option");
                    defaultOption.value = "";
                    defaultOption.style = "color: gray"
                    defaultOption.text = "Select...";
                    positionDropdown.appendChild(defaultOption);

                    data.forEach(position => {
                        let option = document.createElement("option");
                        option.value = position.positionID;
                        option.text = position.positionName;
                        if (positionValue === position.positionID) {
                            option.selected = true;
                        }

                        positionDropdown.appendChild(option);
                    });
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else {
            document.getElementById("updateDpPs").querySelector("#positionLabel").hidden = true;
            document.getElementById("updateDpPs").querySelector("#positionName").hidden = true;
        }
    }

    function getPositionbyDepartment(divName) {
        document.getElementById(divName).querySelector("#positionLabel").hidden = true;
        document.getElementById(divName).querySelector("#positionName").hidden = true;
        document.getElementById(divName).querySelector("#departmentName").onchange = function () {
            let departmentID = this.value;
            let positionDropdown = document.getElementById(divName).querySelector("#positionName");
            positionDropdown.innerHTML = "";

            if (departmentID !== "") {
                $.ajax({
                    url: '/Employee/GetPositionsByDepartment?departmentID=' + departmentID,
                    type: 'GET',
                    success: function (data) {
                        document.getElementById(divName).querySelector("#positionLabel").hidden = false;
                        document.getElementById(divName).querySelector("#positionName").hidden = false;

                        let positionValue = $("#positionValue").attr('value');

                        let defaultOption = document.createElement("option");
                        defaultOption.value = "";
                        defaultOption.style = "color: gray"
                        defaultOption.text = "Select...";
                        positionDropdown.appendChild(defaultOption);

                        data.forEach(position => {
                            let option = document.createElement("option");
                            option.value = position.positionID;
                            option.text = position.positionName;
                            if (positionValue === position.positionID && divName !== "createDpPs") {
                                option.selected = true;
                            }

                            positionDropdown.appendChild(option);
                        });
                    },
                    error: function (error) {
                        console.log(error);
                    }
                });
            } else {
                document.getElementById(divName).querySelector("#positionLabel").hidden = true;
                document.getElementById(divName).querySelector("#positionName").hidden = true;
            }
        }
    }
    

    //Render gender options
    function getGenderOptions(genderOptionId) {
        let genderDropdown = document.getElementById(genderOptionId);
        let genderOptions = [
            { label: "Male", value: "Male" },
            { label: "Female", value: "Female" },
            { label: "Other", value: "Other" }
        ];

        let genderVal = $("#genderValue").attr('value');

        while (genderDropdown.options.length > 0) {
            genderDropdown.remove(0);
        }

        let defaultGender = document.createElement("option");
        defaultGender.value = "";
        defaultGender.style = "color: gray"
        defaultGender.text = "Select...";
        genderDropdown.appendChild(defaultGender);

        
        genderOptions.forEach(option => {
            let genderDropdownOption = document.createElement("option");
            genderDropdownOption.value = option.value;
            genderDropdownOption.text = option.label;
            if (genderVal === option.value && genderOptionId !== "genderCreate") {
                genderDropdownOption.selected = true;
            }

            genderDropdown.appendChild(genderDropdownOption);
        });
    }

    function getGenderOnChange(divGender) {
        document.getElementById(divGender).onchange = function () {
            let genderVal = this.options[this.selectedIndex].label;
            $("#genderValue").attr('value', genderVal);
        }
    }
    
});