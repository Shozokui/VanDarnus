
function onEventStarted(player, npc)
    defaultWil = getStaticActor("DftWil");
    player:runEventFunction("delegateEvent", player, defaultWil, "defaultTalkWithKlamahni_001", nil, nil, nil);
end
