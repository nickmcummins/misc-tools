require 'json'

csv = ARGV[0]

delimeter = if csv.index('.tsv') > 0 then "\t" else ',' end
lines = File.read(csv).split("\n").map{|line| line.split(delimeter)}
header = lines[0]
objects = lines[1..lines.size].map{|line| Hash[header.zip(line)]}
puts JSON.pretty_generate(objects)