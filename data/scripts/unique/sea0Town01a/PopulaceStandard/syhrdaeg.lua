
function onEventStarted(player, npc)
    defaultSea = getStaticActor("DftSea");
    player:runEventFunction("delegateEvent", player, defaultSea, "defaultTalkWithSyhrdaeg_001", nil, nil, nil);
end
