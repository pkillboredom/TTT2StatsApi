SELECT id,
       round_id,
       player_steamid,
       p1.friendly_name AS victim_name,
       killer AS killer_steamid,
       p2.friendly_name AS killer_name,
       death_time,
       death_cause,
       death_flags
  FROM ttt2stats_player_deaths d
       LEFT JOIN
       ttt2stats_players p1 ON p1.steamid = d.player_steamid
       LEFT JOIN
       ttt2stats_players p2 ON p2.steamid = d.killer
 WHERE round_id = @id AND 
       (player_steamid = @steamId OR 
        killer = @steamId);
