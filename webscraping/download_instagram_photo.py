import sys
import requests
from bs4 import BeautifulSoup

url = sys.argv[1]
response = requests.get(url)
pagesoup = BeautifulSoup(response.content, 'html.parser')
img = pagesoup.find('div', {'class': 'KL4Bh'})
print(img)