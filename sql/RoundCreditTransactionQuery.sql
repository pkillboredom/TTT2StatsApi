SELECT id,
round_id,
transaction_type,
credit_amount,
source,
p1.friendly_name AS source_name,
destination, -- if transaction_type is 'equipment_buy'
             -- destination should contain an id for ..._equipment_buy
p2.friendly_name AS dest_name,
source_new_balance
dest_new_balance
FROM ttt2stats_credit_transactions
LEFT JOIN ttt2stats_players p1 ON source = p1.steamid
LEFT JOIN ttt2stats_players p2 ON destination = p2.steamid
WHERE round_id = @roundId;