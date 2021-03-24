# Tubes STIMA 2
## Pengaplikasian Algoritma BFS dan DFS dalam Fitur People You May Know Jejaring Sosial Facebook

                Tugas Besar 2
              Strategi Algoritma
                Dibuat oleh: 
       - Faris Aziz (13519065)
       - M. Fahmi Alamsyah (13519077)
       - Raihan Astrada Fathurrahman (13519113)


## General info
Dalam tugas besar ini, Anda akan diminta untuk membangun sebuah aplikasi GUI sederhana yang dapat memodelkan beberapa fitur dari People You May Know dalam jejaring sosial media (Social Network). Dengan memanfaatkan algoritma Breadth First Search (BFS) dan Depth First
Search (DFS), Anda dapat menelusuri social network pada akun facebook untuk mendapatkan
rekomendasi teman seperti pada fitur People You May Know. Selain untuk mendapatkan
rekomendasi teman, Anda juga diminta untuk mengembangkan fitur lain agar dua akun yang
belum berteman dan tidak memiliki mutual friends sama sekali bisa berkenalan melalui jalur
tertentu.

## Algoritma yang digunakan
1. Breadth First Search adalah algoritma traversal graf yang sering diimplementasikan ketika memiliki representasi data sebagai graf. BFS ini bekerja dengan cara memeriksa seluruh simpul tetangga yang tersedia lalu menyimpannya ke dalam antrian sehingga apabila seluruh tetangga sudah diproses maka antrian yang pertama kali akan dipanggil untuk dilakukan proses traversal kembali

2. Depth First Search adalah algoritma traversal selain bfs yang sering digunakan. Algoritma ini hampir serupa dengan bfs namun untuk menyimpan simpulnya menggunakan stack yang mana stack ini merupakan LIFO. Pertama dilakukan penelusuran terhadap simpul tetangga apabila memenuhi kriteria maka akan dijadikan sebagai simpul yang sekarang lalu akan dilakukan kembali penelusuran tetangga simpul saat ini.

## Technologies
* C#
* Visual Studio 2019

## Setup
1. Install Visual Studio 2019
2. Create project Desktop app

## Cara menggunakan program
1. Jalankan executable code pada folder bin
2. Browse dan pilih file txt yang memuat informasi graph pertemanan
3. Pilih metode search (BFS atau DFS)
4. Pilih features (Friend recommendation atau Explore Friend)
5. Klik tombol submit

## Screenshots
![1fdwfs](https://user-images.githubusercontent.com/49779495/112328765-3470ac00-8ce9-11eb-9574-bfcae468bf07.jpg)

![2hdgdgd](https://user-images.githubusercontent.com/49779495/112328715-2cb10780-8ce9-11eb-8d85-2d8db597f10b.jpg)

## Features
List of features ready:
* Friend Recomendation
* Explore Friend

To-do list for future development :
* Implemetasi ke mobile environment

## Status
Project is: _finished_
