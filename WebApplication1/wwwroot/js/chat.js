$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/Home/Index").build();

    connection.start().then(function () {
        console.log('SignalR Started...')
        //viewModel.roomList();
        //viewModel.userList();
    }).catch(function (err) {
        return console.error(err);
    });

    connection.on("newMessage", function (messageView) {
        //var isMine = messageView.from === viewModel.myName();
        //var message = new ChatMessage(messageView.content, messageView.timestamp, messageView.from, isMine, messageView.avatar);
        console.log("gg");
        //viewModel.chatMessages.push(message);
        console.log("gg1");
        let elem = document.createElement("li");
        elem.textContent = messageView;
        document.getElementById("gg2").appendChild(elem);

        console.log("gg2");
    });

    //connection.on("getProfileInfo", function (displayName, avatar) {
    //    viewModel.myName(displayName);
    //    viewModel.myAvatar(avatar);
    //    viewModel.isLoading(false);
    //});

    //connection.on("onError", function (message) {
    //    viewModel.serverInfoMessage(message);
    //    $("#errorAlert").removeClass("d-none").show().delay(5000).fadeOut(500);
    //});


    function AppViewModel() {
        var self = this;

        self.message = ko.observable("");
        //self.chatRooms = ko.observableArray([]);
        //self.chatUsers = ko.observableArray([]);
        self.chatMessages = ko.observableArray([]);
        //self.joinedRoom = ko.observable("");
        //self.joinedRoomId = ko.observable("");
        //self.serverInfoMessage = ko.observable("");
        self.myName = ko.observable("");
        //self.myAvatar = ko.observable("avatar1.png");
        //self.isLoading = ko.observable(true);

        self.onEnter = function (d, e) {
            if (e.keyCode === 13) {
                self.sendNewMessage();
            }
            return true;
        }
        //self.filter = ko.observable("");
        //self.filteredChatUsers = ko.computed(function () {
        //    if (!self.filter()) {
        //        return self.chatUsers();
        //    } else {
        //        return ko.utils.arrayFilter(self.chatUsers(), function (user) {
        //            var displayName = user.displayName().toLowerCase();
        //            return displayName.includes(self.filter().toLowerCase());
        //        });
        //    }
        //});

        self.sendNewMessage = function () {
            var text = self.message();
            if (text.length > 0) {
                //self.sendPrivate("admin@admin.com", text);
                connection.invoke("Join", text).then(function () {
                    self.myName(text)
                });
                self.sendToRoom(self.message(),self.myName);
                console.log(text);
            }
            else {
                self.message("Error");
                console.log("Error");
                //self.sendToRoom(self.joinedRoom(), self.message());
            }

            self.message("");
        }

        self.sendToRoom = function (message,username) {
            if (message.length > 0) {
                fetch('/Home/News', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ Text: message, UserName: username})
                });
            }
        }

        //self.sendPrivate = function (receiver, message) {
        //    if (receiver.length > 0 && message.length > 0) {
        //        connection.invoke("SendPrivate", receiver.trim(), message.trim());
        //    }
        //}

        //self.joinRoom = function (room) {
        //    connection.invoke("Join", room.name()).then(function () {
        //        self.joinedRoom(room.name());
        //        self.joinedRoomId(room.id());
        //        self.userList();
        //        self.messageHistory();
        //    });
        //}

        //self.roomList = function () {
        //    fetch('/api/Rooms')
        //        .then(response => response.json())
        //        .then(data => {
        //            self.chatRooms.removeAll();
        //            for (var i = 0; i < data.length; i++) {
        //                self.chatRooms.push(new ChatRoom(data[i].id, data[i].name));
        //            }

        //            if (self.chatRooms().length > 0)
        //                self.joinRoom(self.chatRooms()[0]);
        //        });
        //}

        //self.userList = function () {
        //    connection.invoke("GetUsers", self.joinedRoom()).then(function (result) {
        //        self.chatUsers.removeAll();
        //        for (var i = 0; i < result.length; i++) {
        //            self.chatUsers.push(new ChatUser(result[i].username,
        //                result[i].fullName,
        //                result[i].avatar,
        //                result[i].currentRoom,
        //                result[i].device))
        //        }
        //    });
        //}

        //self.createRoom = function () {
        //    var roomName = $("#roomName").val();
        //    fetch('/api/Rooms', {
        //        method: 'POST',
        //        headers: { 'Content-Type': 'application/json' },
        //        body: JSON.stringify({ name: roomName })
        //    });
        //}

        //self.deleteRoom = function () {
        //    fetch('/api/Rooms/' + self.joinedRoomId(), {
        //        method: 'DELETE',
        //        headers: { 'Content-Type': 'application/json' },
        //        body: JSON.stringify({ id: self.joinedRoomId() })
        //    });
        //}
        //TODO посмотреть messagescontroller там будет запрост /api/Messages/Room/
        self.messageHistory = function () {
            fetch('/api/Messages/Room/')
                .then(response => response.json())
                .then(data => {
                    self.chatMessages.removeAll();
                    for (var i = 0; i < data.length; i++) {
                        var isMine = data[i].from == self.myName();
                        self.chatMessages.push(new ChatMessage(data[i].content,
                            data[i].timestamp,
                            data[i].from,
                            isMine,
                            data[i].avatar))
                    }
                    $(".chat-body").animate({ scrollTop: $(".chat-body")[0].scrollHeight }, 1000);
                });
        }

        //self.roomAdded = function (room) {
        //    self.chatRooms.push(room);
        //}

        //self.roomDeleted = function (id) {
        //    var temp;
        //    ko.utils.arrayForEach(self.chatRooms(), function (room) {
        //        if (room.id() == id)
        //            temp = room;
        //    });
        //    self.chatRooms.remove(temp);
        //}

        //self.userAdded = function (user) {
        //    self.chatUsers.push(user);
        //}

        //self.userRemoved = function (id) {
        //    var temp;
        //    ko.utils.arrayForEach(self.chatUsers(), function (user) {
        //        if (user.userName() == id)
        //            temp = user;
        //    });
        //    self.chatUsers.remove(temp);
        //}

        //self.uploadFiles = function () {
        //    var form = document.getElementById("uploadForm");
        //    $.ajax({
        //        type: "POST",
        //        url: '/api/Upload',
        //        data: new FormData(form),
        //        contentType: false,
        //        processData: false,
        //        success: function () {
        //            $("#UploadedFile").val("");
        //        },
        //        error: function (error) {
        //            alert('Error: ' + error.responseText);
        //        }
        //    });
        //}
    }

    //function ChatRoom(id, name) {
    //    var self = this;
    //    self.id = ko.observable(id);
    //    self.name = ko.observable(name);
    //}

    //function ChatUser(userName, displayName, avatar, device) {
    //    var self = this;
    //    self.userName = ko.observable(userName);
    //    self.displayName = ko.observable(displayName);
    //    self.avatar = ko.observable(avatar);
    //    //self.currentRoom = ko.observable(currentRoom);
    //    self.device = ko.observable(device);
    //}

    function ChatMessage(content, timestamp, from, isMine, avatar) {
        var self = this;
        self.content = ko.observable(content);
        self.timestamp = ko.observable(timestamp);
        self.from = ko.observable(from);
        self.isMine = ko.observable(isMine);
        self.avatar = ko.observable(avatar);
    }

    var viewModel = new AppViewModel();
    ko.applyBindings(viewModel);
});
