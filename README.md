# BazePodataka_Projekat_01_SocialNetwork
Studenti :<br />
16178 Pavle Marinković<br />
16753	Ugljesa	Mitrović<br />
16628	Andjela	Jovanović<br />
16597	Djordje	Ivanović<br />
<br />
"Neo4j - Instant Messaging aplikacija
Desktop aplikacija socijalne mreže. Korisnici mogu da kreiraju nalog, dodaju/brisu prijatelje,
pretražuju prijatelje svojih prijatelja, razmenjuju poruke sa prijateljima u realnom vremenu kao i da pretražuju istoriju poruka."
<br />
<br />
Pokretanje projekta :<br />
<br />
1.<br />
Prvo treba pokrenuti Neo4J kroz command prompt<br />
Kroz command prompt odemo u bin folder neo4j distribucije, a zatim pozovemo neo4j console<br />
<br />
2.<br />
Pokrenemo visual studio solution fajl.
U visual studio solution fajlu postoje dva projekta "Server" i "Client"<br />
Prvo treba pokrenuti Server projekat<br />
<br />
3.<br />
Sada možemo pokrenuti koliko god želimo instanci "Client" projekta<br />
<br />
<br />
Za izradu projekta je korišćeno :<br />
<br />
Visual Studio 2019 sa najnovijim .Net-om<br />
<br />
Neo4J distribuiran na vežbama. Verzija 3.3.0<br />
Podešavanja su identična kao u verziji sa vežbi<br />
default databaza se zove graph.db<br />
username : neo4j<br />
password : edukacija<br />
Aplikacija se može koristiti i bez prethodno popunjene databaze,<br />
moguće je napraviti naloge kroz aplikaciju i naći prijatelje.<br />
Ali moguće je i učitati par naloga sa već stoverim relacijama i par izmenjenih poruka koristeći fajl iz repozitorijuma "Database_Dump.dump"<br />
Da bi se dump fajl učitao treba uraditi sledeće,<br />
dok još uvek nije pokrenut neo4j iz command prompt-a pozvati ovu kompandu<br />
<br />
<pre>
neo4j-admin load --from="D:\Database_Dump.dump" --database=graph.db --force<br />
                                ^                             ^<br />
                                |                             |<br />
                                |                         ime databaze (u distribuciji neo4j-a sa vezbi je to graph.db)<br />
           puna putanja do dump fajla iz repozitorijuma<br />
</pre>
<br />
<br />
Više informacija o tome kako importovati dump u neo4j se mogu naći na sledećem linku:<br />
https://neo4j.com/docs/operations-manual/current/tools/dump-load/<br />
<br />
<br />
<br />
<br />
Dodatne informacije:<br />
1.<br />
Klikom na dugme find people se otvara prozor u čiji textbox možemo ukucati username ili deo username-a osobe koju tražimo.<br />
Ako se pronađu nalozi sa sličnim ili istim imenom prikazaće se u listi ispod, nakon čega možemo izabrati nekoga sa liste pretrage i kliknuti na "Send Friend Request"<br />
2.<br />
Imena prijatelja koji su trenutno online su obojeni zelenom bojom, dok su imena prijatelja koji su offline obojeni crnom bojom<br />
3.<br />
Moguće je uraditi double-click na ime prijatelja i time će se otvoriti prozor sa listom prijatelja tog našeg prijatelja<br />
5.<br />
Ako nam nije otvoren chat sa osobom koja nam je upravo poslala poruku, boja pozadine stavke u listi prijatelja će se promeniti<br />
6.<br />
Brisanjem nekoga iz naše liste prijatelja pokreće proceduru kojom se prvo brišu svi ChatEntry nodovi, CHAT_ENTRY relacije, Chat node, CHAT relacije i FRIEND relacije<br />
izmedju dva Account node-a<br />
<br />
<img src="https://i.imgur.com/dIBgxAb.png"/>
