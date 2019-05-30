require("global");
require("weaponskill");

function onSkillPrepare(caster, target, skill)
    return 0;
end;

function onSkillStart(caster, target, skill)
    return 0;
end;

--Dispel
--Does dispel have a text id?
function onCombo(caster, target, skill)
    return 0;
end;

function onSkillFinish(caster, target, skill, action, actionContainer)
    --calculate ws damage
    action.amount = skill.basePotency;

    --DoAction handles rates, buffs, dealing damage
    action.DoAction(caster, target, skill, actionContainer);

    if skill.isCombo then
        target.statusEffects.RemoveStatusEffect(GetRandomEffectByFlag(8), actionContainer, 30336);
    end
end;