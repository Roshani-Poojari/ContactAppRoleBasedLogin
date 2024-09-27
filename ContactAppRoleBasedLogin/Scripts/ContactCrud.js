//show contact table
function loadContacts() {
    $.ajax({
        url: "/Contact/ViewContacts",
        type: "GET",
        success: function (data) {
            $("#tblBody").empty()

            $.each(data, function (index, item) {
                var checkbox = `<input type="checkbox" class="is-active-checkbox" data-id="${item.Id}" ${item.IsActive ? 'checked' : ''} />`;
                var row = `<tr>
                <td>${item.FirstName}</td>
                <td>${item.LastName}</td>
                <td>${checkbox}</td>
                <td>
                <button onclick="editContact('${item.Id}')" class="btn btn-outline-dark">Edit</button>
                </td>
                <td>
                <button onclick="viewContactDetails('${item.Id}')" class="btn btn-outline-dark">View Contact Details</button>
                </td>
                </tr>`
                $("#tblBody").append(row)
            })
            $(".is-active-checkbox").change(function () {
                var contactId = $(this).data("id");
                var isActive = $(this).is(":checked");
                updateContactStatus(contactId, isActive);
            });
        },

        error: function (err) {
            $("#tblBody").empty()
            alert("No data available")
        }

    })
}

//view contact details
function viewContactDetails(contactId) {
    window.location.href = `/ContactDetail/Index?contactId=${contactId}`;
}

//toggle isactive(soft delete)
function updateContactStatus(contactId, isActive) {
    $.ajax({
        url: "/Contact/UpdateContactStatus",
        type: "POST",
        data: { contactId: contactId, isActive: isActive },
        success: function () {
            alert("Contact status updated successfully");
        },
        error: function () {
            alert("Error updating contact status");
        }
    });
}

//add new contact
function addNewContact() {
    var newContact = {
        FirstName: $("#fname").val(),
        LastName: $("#lname").val()
    };

    $.ajax({
        url: "/Contact/CreateContact",
        type: "POST",
        data: newContact,
        success: function (response) {
            if (response.success === false) {
                alert("Errors: " + response.errors.join(",   "));
                return;
            }
            alert("New contact added successfully");
            loadContacts();
            $("#newContact").hide();
            $("#contactList").show();
        },
        error: function (err) {
            alert("Error adding new contact");
            console.log(err);
        }
    });
}

//getContactById
function getContact(contactId) {
    $.ajax({
        url: "/Contact/GetContactById",
        type: "GET",
        data: { contactId: contactId },
        success: function (response) {
            if (response.success) {
                $("#contactId").val(response.contact.Id)
                $("#editedFname").val(response.contact.FirstName)
                $("#editedLname").val(response.contact.LastName)
            }
            else {
                alert(response.message)
            }
        },
        error: function (err) {
            alert("No such contact found")
        }
    })
}


//edit contact button on click
function editContact(contactId) {
    getContact(contactId);
    $("#contactList").hide();
    $("#editContact").show();
}

//edit contact main function
function modifyContact() {
    var modifiedContact = {
        Id: $("#contactId").val(),
        FirstName: $("#editedFname").val(),
        LastName: $("#editedLname").val()
    }
    $.ajax({
        url: "/Contact/EditContact",
        type: "POST",
        data: modifiedContact,
        success: function (response) {
            if (response.success === false) {
                alert("Errors: " + response.errors.join(",   "));
                return;
            }
            alert("Contact edited successfully");
            loadContacts();
            $("#editContact").hide();
            $("#contactList").show();
        },
        error: function (err) {
            alert("Error updating contact");
            console.log(err);
        }
    });
}


