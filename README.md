# Instru��es para conseguir rodar o projeto e testa-lo 


# Para testes, obrigat�rio ter uma m�quina virtual, porque para testes o servidor precisa de 2 m�quinas conectadas a ele.

# 1 - Ter uma m�quina virtual com Windows (a partir do Windows 10 pra cima)
# 2 - Instalar o Visual Studio 2022 Community Edition
# 3 - Clonar o reposit�rio na m�quina virtual

# Rodando o projeto:

# 1 - Quando o projeto for iniciado (provavelmente ele vai abrir os dois, client e server) na maquina virtual...
# n�o a necessidade de inicializar o servidor, pois ele j� vai estar rodando na maquina local.

# 2 - Os ip's do servidor que est�o funcionando para o cliente se conectar ao servidor s�o ip's nesse padr�o...
# 26.24.181.221
# 192.168.56.1
# 192.168.22.43
# A porta em seguida do ip est� configurada para ser 11000, a forma de digitar o ip para se conectar...
# segue esse padr�o 192.168.56.1:11000

# O problema que est� tendo nesse projeto � que quando o player 2 se conectava, o servidor n�o esperava...
# ele inserir o Nickname e j� iniciava a UI.