SELECT id,
       damage_time,
       players1.friendly_name AS attacker_name,
       attacker_steamid,
       players2.friendly_name AS victim_name,
       victim_steamid,
       damage_dealt,
       health_remain,
       weapon
  FROM ttt2stats_player_damage dmg
       LEFT JOIN
       ttt2stats_players players1 ON players1.steamid = dmg.attacker_steamid
       LEFT JOIN
       ttt2stats_players players2 ON players2.steamid = dmg.victim_steamid
 ORDER BY damage_time DESC
 LIMIT @rowLimit;
