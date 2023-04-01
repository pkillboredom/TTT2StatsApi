SELECT karm.round_id AS round_id,
       p.steamid AS player_steamid,
       p.friendly_name,
       rt.player_role,
       karm.player_starting_karma,
       karm.player_ending_karma,
       COUNT(k.id) AS kills,
       COUNT(d.id) AS deaths
  FROM ttt2stats_players p
       JOIN
       ttt2stats_player_round_karma karm ON p.steamid = karm.player_steamid AND 
                                            karm.round_id = @roundId
       LEFT JOIN
       ttt2stats_player_round_roles rt ON p.steamid = rt.player_steamid AND 
                                          rt.round_id = @roundId
       LEFT JOIN
       ttt2stats_player_deaths d ON p.steamid = d.player_steamid AND 
                                    d.round_id = @roundId
       LEFT JOIN
       ttt2stats_player_deaths k ON p.steamid = k.killer AND 
                                    k.round_id = @roundId
 GROUP BY p.steamid;
