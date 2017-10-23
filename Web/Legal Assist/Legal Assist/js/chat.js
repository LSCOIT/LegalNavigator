$(document).ready(function () {
    
     ValidateContent('down');
});

$(document).on('click', '.panel-heading span.icon_minim', function (e) {
    var $this = $(this);
    if (!$this.hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideUp();
        $this.addClass('panel-collapsed');
        $this.removeClass('glyphicon-minus').addClass('glyphicon-plus');
    } else {
        $this.parents('.panel').find('.panel-body').slideDown();
        $this.removeClass('panel-collapsed');
        $this.removeClass('glyphicon-plus').addClass('glyphicon-minus');
    }
});
$(document).on('focus', '.panel-footer input.chat_input', function (e) {
    var $this = $(this);
    if ($('#minim_chat_window').hasClass('panel-collapsed')) {
        $this.parents('.panel').find('.panel-body').slideDown();
        $('#minim_chat_window').removeClass('panel-collapsed');
        $('#minim_chat_window').removeClass('glyphicon-plus').addClass('glyphicon-minus');
    }
});
$(document).on('click', '#new_chat', function (e) {
    var size = $(".chat-window:last-child").css("margin-left");
    size_total = parseInt(size) + 400;
    alert(size_total);
    var clone = $("#chat_window_1").clone().appendTo(".container");
    clone.css("margin-left", size_total);
});
$(document).on('click', '.icon_close', function (e) {
      $("#chat_window_1").remove();
});

function InputChatMsg() {
    var chatvalue = "<div class='row msg_container base_receive' id='InputChat'><div class='col-md-10 col-xs-10 col-md-offset-1 col-xs-offset-1'><div class='messages msg_sent'><p>" + $('#btn-input').val() + "</p><time datetime='2009-11-13T20:00'>Sue • 51 min</time></div></div></div>";
    $('#divChatControl').append(chatvalue);
    $('#btn-input').val('');
    $('#divChatControl').scrollTop($('#divChatControl')[0].scrollHeight);
}

function InputChatMsgRobo(val) {
    var chatvalue = "<div class='row msg_container base_receive' id='InputChat'><div class='col-md-10 col-xs-10 col-md-offset-1 col-xs-offset-1'><div class='messages msg_sent'><p>" + val + "</p><time datetime='2009-11-13T20:00'>Sue • 51 min</time></div></div></div>";
    $('#divChatControl').append(chatvalue);
    $('#btn-input').val('');
    //$('#divChatControl').scrollTop($('#divChatControl')[0].scrollHeight);
  }

function RecieveChatMsg(val) {
    var chatvalue = "<div class='row msg_container base_sent' id='InputChat'><div class='col-md-10 col-xs-10 col-md-offset-1 col-xs-offset-1'><div class='messages msg_receive'><p>" + val + "</p><time datetime='2009-11-13T20:00'>Timothy • 51 min</time></div></div></div>";
    $('#divChatControl').append(chatvalue);
    $('#btn-input').val('');
    $('#divChatControl').scrollTop($('#divChatControl')[0].scrollHeight);
}

function InputReference(url,val)
{
   var refvalue = "<div class='row msg_container base_receive'><div class='col-md-10 col-xs-10'><div class='chat-m-box'><button type='button' class='close'><i class='fa fa-times'></i></button><img src='images/chat-text.png' width='36' height='39' class='chat-m-box-img'/>"+
        "<div class='f-s-12px'><a href="+url+" target='_blank'>"+val+"</a></div></div></div></div>";
   $('#divRefControl').append(refvalue);
   $('#divRefControl').scrollTop($('#divRefControl')[0].scrollHeight);
}

setTimeout(function () {
    InputChatMsgRobo('I am being evicted');
}, 5000);

setTimeout(function () {
    RecieveChatMsg('Have you received a notice?');
}, 10000);

setTimeout(function () {
    InputChatMsgRobo('Yes, i have');
}, 15000);

setTimeout(function () {
    RecieveChatMsg('Do you understand the notice?');
}, 25000);

setTimeout(function () {
    InputChatMsgRobo('No, Im confused');
}, 30000);

setTimeout(function () {
    RecieveChatMsg('Can you scan your document?');
}, 35000);

setTimeout(function () {
    InputChatMsgRobo('Yes, I have access to scanner');
}, 45000);

setTimeout(function () {
    InputChatMsgRobo('I have scanned the document');
}, 55000);

setTimeout(function () {
    RecieveChatMsg('Great! Let me have alook at it');
}, 65000);

setTimeout(function () {
    InputChatMsgRobo('Thank you');
}, 75000);

setTimeout(function () {
    RecieveChatMsg('Are you having trouble making payments?');
}, 80000);

setTimeout(function () {
    InputChatMsgRobo('yes I’m unemployed and I had a fight with my boyfriend and he left me and my child.');
}, 85000);

//right side entries
setTimeout(function () {
    InputReference('http://www.washingtonlawhelp.org/issues/family-law/domestic-violence-1', 'Domestic Violence - Washington Law Help');
}, 5000);

setTimeout(function () {
    InputReference('https://law.marquette.edu/assets/community/pdf/mvlc-ClientEvictionHandout.pdf', 'What you need to know about eviction.pdf');
}, 10000);

setTimeout(function () {
    InputReference('http://ptla.org/what-can-i-do-if-my-landlord-trying-evict-me', 'What Can I Do if My Landlord is Trying to Evict Me? | Pine Tree Legal...');
}, 15000);

setTimeout(function () {
    InputReference('http://www.tenantsunion.org/en/rights/eviction-process', 'Eviction Process - Know Your Rights - Tenants Union of Washington...');
}, 20000);

setTimeout(function () {
    InputReference('http://www.nolo.com/legal-encyclopedia/evictions-renters-tenants-rights-29824.html', 'How Evictions Work: What Renters Need to Know | Nolo.com...');
}, 25000);

localStorage.setItem("returnSession", "chat.html");

  